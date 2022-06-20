using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ZombieManager : MonoBehaviourPunCallbacks
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
        for (int i = GameObject.FindGameObjectsWithTag("Enemy").Length; i < 100; i++)
        {
            int a = Random.Range(0, spawnPoints.Length);
            GameObject newZombie = PhotonNetwork.InstantiateRoomObject(zombie.name, new Vector3(spawnPoints[a].transform.position.x, spawnPoints[a].transform.position.y, spawnPoints[a].transform.position.z), Quaternion.identity);
            EnemyMovement enemyMovement = newZombie.GetComponent<EnemyMovement>();
            enemyMovement.waypoints = GameObject.Find("Spawner Manager").GetComponent<SpawnerManager>().waypoints;
            yield return new WaitForSeconds(1);
        }
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        PhotonNetwork.LeaveRoom();
    }
}
