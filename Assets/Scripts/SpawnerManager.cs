using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    //Settings
    public type TriggerType;
    public enum type { Player, Train };
    public float spawnTime;
    public GameObject[] enemyPrefabs;
    //Holds the max number of enemies allowed at any one time
    public int maxNumberOfEnemiesAtOneTime = 8;
    public int maxNumberOfEnemiesOtherSpawnerLifeTime = 16;
    public int distancePlayerMustBeFromSpawnerBeforeSpawnerInstantiates = 15;
    //Creates an option allowing us to enable the spawner when the player passes through a trigger
    public bool enableSpawnerByTrigger = false;
    //public GameObject door;
    public AudioClip doorOpenAudio;

    //Lists
    private List<Transform> spawnPoints = new List<Transform>(); //List for spawnpoints
    [HideInInspector]
    public List<Transform> waypoints = new List<Transform>(); //List for waypoints
    //List for enemies this spawner has created that are still alive
    [HideInInspector] //Hides var below
    public List<GameObject> enemiesFromThisSpawnerList = new List<GameObject>();

    //General
    private int enemycount;
    private GameObject player;
    private bool enableSpawner = true;
    private bool enableDoors;
    private bool openDoor = false;
    private AudioSource doorAudioSource;

    public void Awake()
    {
        doorAudioSource = GetComponent<AudioSource>();
        openDoor = false;
    }

    public void OnDrawGizmos()
    {
        foreach (Transform child in transform)
        {
            if (child.tag == "Waypoint")
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(child.position, .8f);
            }
            if (child.tag == "SpawnPoint")
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(child.position, .8f);
            }
            if (child.tag == "SpawnTrigger")
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawCube(child.position, child.transform.localScale);
            }
        }
    }
    // Start is called before the first frame update
    public void Start()
    {
        SetUpChildObjects();
        player = GameObject.FindGameObjectWithTag("Player");

        //if (door)
        //{
        //doorAudioSource = door.AddComponent<AudioSource>();
        //}
        InvokeRepeating("checkIfObjectShouldBeSpawned", spawnTime, spawnTime);
    }
    //Checks the setup child elements in the spawner
    public void SetUpChildObjects()
    {
        //Adds Spawn Points and Waypoints to their appropriate lists
        foreach (Transform child in transform)
        {
            if (child.tag == "Waypoint")
            {
                waypoints.Add(child);
            }
            if (child.tag == "SpawnPoint")
            {
                spawnPoints.Add(child);
            }
            //If enableSpawnerByTrigger is false, destroy all Spawntrigger children
            if (child.tag == "SpawnTrigger" && enableSpawnerByTrigger == false)
            {
                Destroy((child).gameObject); //The SpawnTrigger Child can not be destroyed as a transform so it needs to be referenced
            }
            else if (child.tag == "SpawnTrigger" && enableSpawnerByTrigger == true)
            {
                enableSpawner = false; //Disables the spawner if the user wished to use a trigger to enable the spawner.
            }
            //if (door)
            //{
            //enableDoors = true;
            //}
            //else
            //{
            //enableDoors = false;
            //}
        }
    }
    //If there is a Door the door is opened if all enemies have been destroyed
    private void FixedUpdate()
    {
        //if (openDoor)
        {
            //door.transform.position -= new Vector3(0, 1f, 0) * Time.fixedDeltaTime;
            //if (door.transform.position.y < -8f)
            //{
            //Destroy(gameObject); //Not using GameController.gameController.SpawnerDestroyed(gameObject);
            //}
        }
    }
    //If we are using trigger to enable the spawner. The spawner is enabled when the player triggers it
    public void OnTriggerEnter(Collider other)
    {
        if (!enableSpawnerByTrigger)
        {
            return;
        }
        else if (other.tag == "Player" && !openDoor)
        {
            if (TriggerType == type.Player)
            {
                doorAudioSource.clip = doorOpenAudio;
                doorAudioSource.Play();
                enableSpawner = true;
            }
        }
        else if (other.tag == "Train")
        {
            if (TriggerType == type.Train)
            {
                enableSpawner = true;
            }
        }
        openDoor = true;
    }
    //Checks if object should be spawned
    public void checkIfObjectShouldBeSpawned()
    //Allows us to disable and enable the spawner
    {
        if (enableSpawner == true && !HUDManager.gamePaused)
        {
            //If we have not reached the limit of enemies from this spawner
            if (enemiesFromThisSpawnerList.Count < maxNumberOfEnemiesAtOneTime && enemycount < maxNumberOfEnemiesOtherSpawnerLifeTime)
            {
                EnemySetActive();
            }
            //If we have reached the max number of enemies and all the enemies from this spawner are dead
            else if (enemiesFromThisSpawnerList.Count == 0 && enemycount >= maxNumberOfEnemiesOtherSpawnerLifeTime)
            {
                DestroySpawner();
            }
        }
    }
    //When this function is called, an enemy is instantiated
    void EnemySetActive()
    {
        //If the distance between the spawn position and the player position is bigger than 15
        if (Vector3.Distance(spawnPoints[0].position, player.transform.position) > distancePlayerMustBeFromSpawnerBeforeSpawnerInstantiates)
        {
            enemycount++; //adds one to the enemycount
            int a = Random.Range(0, spawnPoints.Count);
            //Set a to a random spawn position
            int b = Random.Range(0, enemyPrefabs.Length);
            //Spawn enemy
            GameObject returnedGameObject = PoolManager.current.GetPooledObject(enemyPrefabs[b].name);
            if (returnedGameObject == null) return;
            returnedGameObject.transform.position = spawnPoints[a].position;
            returnedGameObject.transform.rotation = spawnPoints[a].rotation;
            returnedGameObject.SetActive(true);
            //Passes the waypoints list to the new enemy script
            EnemyMovement enemyMovement = returnedGameObject.GetComponent<EnemyMovement>();
            enemyMovement.waypoints = waypoints;

            //Loads a reference to the EnemySpawner Script into the health script so that we can remove objects from the list
            EnemyHealth enemyHealth = returnedGameObject.GetComponent<EnemyHealth>();
            enemyHealth.enemySpawnerScript = GetComponent<SpawnerManager>();

            //Allows us to track the number of enemies currently alive
            enemiesFromThisSpawnerList.Add(returnedGameObject); //Adds enemy count to the enemiesFromThisSpawnerList
            //Adds the enemy to the main enemy list in the GameController
            GameController.gameController.enemies.Add(returnedGameObject);
        }
    }
    void DestroySpawner()
    {
        if (enableDoors)
        {
            //if (doorOpenAudio && door && !openDoor)
            //{
            //doorAudioSource.clip = doorOpenAudio;
            //doorAudioSource.Play();
            //}
            openDoor = true;
        }
        else
        {
            Destroy(gameObject); //Not using Gamecontroller.gamecontroller.SpawnerDestroyed(gameObject);
        }
    }
}
