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

    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            GameObject.Find("RoundCanvas").GetComponent<RoundManager>().UpdateScore(other.GetComponent<PlayerInputs>().playerNum, other.GetComponent<CoinValueHeld>().coinValueHeld);
            //roundManager.UpdateScore(other.GetComponent<PlayerInputs>().playerNum, other.GetComponent<CoinValueHeld>().coinValueHeld);
            other.GetComponent<CoinValueHeld>().coinValueHeld = 0;
            other.GetComponentInChildren<HUDManager>().UpdateHUD();
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
