using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

public class EnemyMovement : MonoBehaviour
{
    //Settings
    public float enemySpeed = 5f;
    public float enemyStoppingDistance = 1f;
    private bool playerInRange = false;
    //private GameObject[] player;
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
    private int wayPointIndex; //Stores the waypoint key that the enemy will move towards.
    [HideInInspector]
    public List<Transform> waypoints = new List<Transform>(); //List info parsed from SpawnerManager Script

    private void Awake()
    {
        if (!PhotonNetwork.IsConnected || PhotonNetwork.IsConnected && PhotonNetwork.IsMasterClient)
        {
            anim = GetComponentInChildren<Animator>();
            agent = GetComponent<NavMeshAgent>();
            enemyRigidbody = GetComponent<Rigidbody>();
            enemyCollider = GetComponent<Collider>();
            enemyHealth = GetComponent<EnemyHealth>();
            convergeOnPlayer = false;
            //Invoke("FindPlayer", 1f);
        }
    }

    private void FindPlayer()
    {
        //player = GameObject.FindGameObjectsWithTag("Player");
    }

    private void OnEnable()
    {
        if (!PhotonNetwork.IsConnected || PhotonNetwork.IsConnected && PhotonNetwork.IsMasterClient)
        {
            agent.updatePosition = true; //Stops the enemy from moving
            agent.updateRotation = true; //Stops the enemy from rotating
            enemyRigidbody.isKinematic = false;
            enemyCollider.enabled = true;
            agent.enabled = true;
            deathChecked = false;
        }
    }

    public void Update()
    {
        if (!PhotonNetwork.IsConnected || PhotonNetwork.IsConnected && PhotonNetwork.IsMasterClient)
        {
            if (!playerInRange)
            {
                Waypoints();
            }
        }
    }

    public void FixedUpdate()
    {
        if (!PhotonNetwork.IsConnected || PhotonNetwork.IsConnected && PhotonNetwork.IsMasterClient)
        {
            NavMeshHit hit;

            if (!NavMesh.Raycast(transform.position, transform.forward, out hit, 1) && !sendEnemiesBackToWayPoints || enemyHealth.enemyWasShotAtByThePlayer && !sendEnemiesBackToWayPoints || convergeOnPlayer)
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
    }

    public void OnTriggerEnter(Collider other)
    {
        if (!PhotonNetwork.IsConnected || PhotonNetwork.IsConnected && PhotonNetwork.IsMasterClient)
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
    }

    public void OnTriggerStay(Collider other)
    {
        if (!PhotonNetwork.IsConnected || PhotonNetwork.IsConnected && PhotonNetwork.IsMasterClient)
        {
            if (other.CompareTag("Player"))
            {
                other.GetComponent<PlayerHealth>();

                if (!other.GetComponent<PlayerHealth>().isDead)
                {
                    playerInRange = true;
                    anim.SetBool("Run", true);
                    anim.SetBool("Walk", false);
                    NavMeshAgentSettings(other.transform.position, enemySpeed, enemyStoppingDistance); //Sets the destination for navmesh
                }
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (!PhotonNetwork.IsConnected || PhotonNetwork.IsConnected && PhotonNetwork.IsMasterClient)
        {
            if (other.CompareTag("Player"))
            {
                playerInRange = false;
            }
        }
    }


    public void UpdatePosition()
    {
        if (!PhotonNetwork.IsConnected || PhotonNetwork.IsConnected && PhotonNetwork.IsMasterClient)
        {
            agent.updatePosition = true;

            NavMeshHit hit;

            if (NavMesh.SamplePosition(transform.position, out hit, 50, 1))
            {
                agent.Warp(hit.position);
            }
            else
            {
                enemyHealth.Death();
            }
        }
    }  

    public void NavMeshAgentSettings(Vector3 enemyTarget, float enemySpeed, float enemyStoppingDistance)
    {
        if (!PhotonNetwork.IsConnected || PhotonNetwork.IsConnected && PhotonNetwork.IsMasterClient)
        {
            if (agent.isOnNavMesh && !HUDManager.gamePaused)
            {
                agent.SetDestination(enemyTarget); //Moves enemy to the Target
                agent.speed = enemySpeed; //Sets the chase speed
                agent.stoppingDistance = enemyStoppingDistance;
            }
        }
    }

    public void Waypoints()
    {
        if (!PhotonNetwork.IsConnected || PhotonNetwork.IsConnected && PhotonNetwork.IsMasterClient)
        {
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
                anim.SetBool("Run", false);
                anim.SetBool("Walk", true);
                NavMeshAgentSettings(waypoints[wayPointIndex].position, enemyWaypointSpeed, enemyWaypointStoppingDistance);
            }
        }
    }
}
