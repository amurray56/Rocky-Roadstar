using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ZombieManager : MonoBehaviour
{

    public GameObject zombie;
    private GameObject[] spawnPoints;

    // Start is called before the first frame update
    void Start()
    {
        spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
        if(PhotonNetwork.IsMasterClient)
        StartCoroutine(SpawnZombie());
    }

    IEnumerator SpawnZombie()
    {
        WaitForSeconds wait = new WaitForSeconds(1);

        for (int i = 0; i < 100; i++)
        {
            int a = Random.Range(0, spawnPoints.Length);
            GameObject newZombie = PhotonNetwork.Instantiate(zombie.name, new Vector3(spawnPoints[a].transform.position.x, spawnPoints[a].transform.position.y, spawnPoints[a].transform.position.z), Quaternion.identity);
            EnemyMovement enemyMovement = newZombie.GetComponent<EnemyMovement>();
            enemyMovement.waypoints = GameObject.Find("Spawner Manager").GetComponent<SpawnerManager>().waypoints;
            yield return wait;
        }
        
    }
}
