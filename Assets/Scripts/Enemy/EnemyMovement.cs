using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    //Settings
    public float enemySpeed = 5f;
    public float enemyStoppingDistance = 1f;
    private GameObject[] player;
    [HideInInspector]
    public Transform enemyTargetP1; //Holds the target that the enemies will move towards
    public Transform enemyTargetP2;
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
    private NavMeshHit hit;

    //Waypoints
    public float enemyWaypointSpeed = 3f;
    public float enemyWaypointStoppingDistance = 2f;
    private int wayPointIndex; //Stores the waypoint key that the enemy will move towards.
    [HideInInspector]
    public List<Transform> waypoints = new List<Transform>(); //List info parsed from SpawnerManager Script

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();
        enemyRigidbody = GetComponent<Rigidbody>();
        enemyCollider = GetComponent<Collider>();
        enemyHealth = GetComponent<EnemyHealth>();
        convergeOnPlayer = false;
        Invoke("FindPlayer", 1f);
    }

    private void FindPlayer()
    {
        player = GameObject.FindGameObjectsWithTag("Player");
        enemyTargetP1 = player[0].transform; //Sets the enemy target to Player
        enemyTargetP2 = player[1].transform;
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

        if (!NavMesh.Raycast(transform.position, enemyTargetP1.position, out hit, -1))
        {
            enemyHealth.enemyWasShotAtByThePlayer = false;
        }

        if (!NavMesh.Raycast(transform.position, enemyTargetP2.position, out hit, -1))
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

        if (NavMesh.SamplePosition(transform.position, out hit, 500, 1))
        {
            agent.Warp(hit.position);
        }
        else
        {
            enemyHealth.Death();
        }
    }

    void EnemyMovementAI()
    {
        //If the enemy can see the player or the enemy was shot by the player or converge on player is true
        if ((!NavMesh.Raycast(transform.position, enemyTargetP1.position, out hit, -1)) && !GetComponent<PlayerHealth>().isDead && !sendEnemiesBackToWayPoints || enemyHealth.enemyWasShotAtByThePlayer && !GetComponent<PlayerHealth>().isDead && !sendEnemiesBackToWayPoints || convergeOnPlayer && !GetComponent<PlayerHealth>().isDead)
        {
            NavMeshAgentSettings(enemyTargetP1.position, enemySpeed, enemyStoppingDistance); //Sets the destination for navmesh
            anim.SetBool("Run", true);
            anim.SetBool("Walk", false);
        }
        else
        {
            Waypoints();
            anim.SetBool("Run", false);
            anim.SetBool("Walk", true);
        }

        if ((!NavMesh.Raycast(transform.position, enemyTargetP2.position, out hit, -1)) && !GetComponent<PlayerHealth>().isDead && !sendEnemiesBackToWayPoints || enemyHealth.enemyWasShotAtByThePlayer && !GetComponent<PlayerHealth>().isDead && !sendEnemiesBackToWayPoints || convergeOnPlayer && !GetComponent<PlayerHealth>().isDead)
        {
            NavMeshAgentSettings(enemyTargetP2.position, enemySpeed, enemyStoppingDistance); //Sets the destination for navmesh
            anim.SetBool("Run", true);
            anim.SetBool("Walk", false);
        }
        else
        {
            Waypoints();
            anim.SetBool("Run", false);
            anim.SetBool("Walk", true);
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
            //NavMeshAgent.SetPath(waypoints[wayPointIndex].position);
        }
    }
}
