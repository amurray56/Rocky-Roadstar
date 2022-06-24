using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ScoreZone : MonoBehaviour
{
    //private GameObject roundCanvas;
    //private RoundManager roundManager;


    private void Start()
    {
        //roundCanvas = GameObject.Find("RoundCanvas");
        //roundManager = roundCanvas.GetComponent<RoundManager>();
        GetComponent<BoxCollider>().isTrigger = true;
    }

    public void OnTriggerEnter(Collider player)
    {
        if(player.CompareTag("Player"))
        {
            //if (PhotonNetwork.IsConnected)
                //GameObject.Find("RoundCanvas").GetComponent<RoundManager>().photonView.RPC("UpdateScore", RpcTarget.Others, player.GetComponent<PlayerInputs>().playerNum, player.GetComponent<CoinValueHeld>().coinValueHeld);
            GameObject.Find("RoundCanvas").GetComponent<RoundManager>().UpdateScore(player.GetComponent<PlayerInputs>().playerNum, player.GetComponent<CoinValueHeld>().coinValueHeld);
            //roundManager.UpdateScore(other.GetComponent<PlayerInputs>().playerNum, other.GetComponent<CoinValueHeld>().coinValueHeld);
            player.GetComponent<CoinValueHeld>().coinValueHeld = 0;
            player.GetComponentInChildren<HUDManager>().UpdateHUD();
            Invoke("EndCheck", .5f);
        }
    }

    public void EndCheck()
    {
        if (PhotonNetwork.IsConnected)
            GameObject.Find("RoundCanvas").GetComponent<RoundManager>().photonView.RPC("CheckForEnd", RpcTarget.All);
        else
            GameObject.Find("RoundCanvas").GetComponent<RoundManager>().CheckForEnd();
    }
}
