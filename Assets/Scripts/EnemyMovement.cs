using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    //Settings
    public float enemySpeed = 5f;
    public float enemyStoppingDistance = 1f;
    private Transform player;
    [HideInInspector]
    public Transform enemyTarget; //Holds the target that the enemies will move towards
    [HideInInspector]
    public static bool convergeOnPlayer = false; //When this value is set to true the enemies will converge on the player
    public static bool sendEnemiesBackToWayPoints = false; //Send enemies back to their waypoints
    private bool deathChecked = false; //This is set to true if the enemy falls off the edge
    public float fallDistanceBeforeDeath = -10f; //Enemy fall distance before death

    //General Components
    private NavMeshAgent agent;
    private Collider enemyCollider;
    private Rigidbody enemyRigidbody;
    private Animator anim;
    private EnemyHealth enemyHealth;

    //Waypoints
    public float enemyWaypointSpeed = 3f;
    public float enemyWaypointStoppingDistance = 2f;
    private int wayPointIndex = 0; //Stores the waypoint key that the enemy will move towards.
    [HideInInspector]
    public List<Transform> waypoints = new List<Transform>();

    private void Awake()
    {
        if (tag != "Enemy")
        {
            tag = "Enemy";
        }

        anim = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();
        enemyRigidbody = GetComponent<Rigidbody>();
        enemyCollider = GetComponent<Collider>();
        player = GameObject.Find("Player").transform;
        enemyHealth = GetComponent<EnemyHealth>();
        convergeOnPlayer = false;
        enemyTarget = player; //Sets the enemy target to Player
    }

    private void OnEnable()
    {
        agent.updatePosition = true; //Stops the enemy from moving
        agent.updateRotation = true; //Stops the enemy from rotating
        enemyRigidbody.isKinematic = false;
        enemyCollider.enabled = true;
        agent.enabled = true;
        deathChecked = false;
    }

    public void FixedUpdate()
    {
        EnemyMovementAI();

        NavMeshHit hit;
        if (!NavMesh.Raycast(transform.position, enemyTarget.position, out hit, -1))
        {
            enemyHealth.enemyWasShotAtByThePlayer = false;
        }

        if (HUDManager.gamePaused || HUDManager.lose || HUDManager.victory)
        {
            agent.isStopped = true;
        }
        else
        {
            agent.isStopped = false;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (agent.updateRotation == false)
        {
            if (enemyHealth.enemyHealthAmount > 0)
            {
                agent.updateRotation = true;
                anim.SetBool("Explosion", false);
                Invoke("UpdatePosition", 0.8f);
            }
        }
    }

    public void UpdatePosition()
    {
        agent.updatePosition = true;
        NavMeshHit closestHit;

        if (NavMesh.SamplePosition(transform.position, out closestHit, 500, 1))
        {
            agent.Warp(closestHit.position);
        }
        else
        {
            enemyHealth.Death();
        }
    }

    void EnemyMovementAI()
    {
        NavMeshHit hit;
        //If the enemy can see the player or the enemy was shot by the player or converge on player is true
        if ((!NavMesh.Raycast(transform.position, enemyTarget.position, out hit, -1)) && !PlayerHealth.isDead && !sendEnemiesBackToWayPoints || enemyHealth.enemyWasShotAtByThePlayer && !PlayerHealth.isDead && !sendEnemiesBackToWayPoints || convergeOnPlayer && !PlayerHealth.isDead)
        {
            NavMeshAgentSettings(enemyTarget.position, enemySpeed, enemyStoppingDistance); //Sets the destination for navmesh
        }
        else
        {
            Waypoints();
        }
        if (transform.position.y < fallDistanceBeforeDeath && deathChecked == false)
        {
            deathChecked = true;
            enemyHealth.Death();
        }
        if (agent.updatePosition == true && !agent.isOnNavMesh)
        {
            enemyHealth.Death();
        }
    }

    public void NavMeshAgentSettings(Vector3 enemyTarget, float enemySpeed, float enemyStoppingDistance)
    {
        if (agent.isOnNavMesh && !HUDManager.gamePaused)
        {
            agent.SetDestination(enemyTarget); //Moves enemy to the Target
            agent.speed = enemySpeed; //Sets the chase speed
            agent.stoppingDistance = enemyStoppingDistance;
        }
    }

    public void Waypoints()
    {
        //Debug.Log(waypoints.Count);
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            if (wayPointIndex < waypoints.Count - 1)
            {
                wayPointIndex++;
            }
            else
            {
                wayPointIndex = 0;
            }
        }
        if (waypoints == null)
        {
            enemyHealth.Death();
        }
        else
        {
            NavMeshAgentSettings(waypoints[wayPointIndex].position, enemyWaypointSpeed, enemyWaypointStoppingDistance);
        }
    }
}
