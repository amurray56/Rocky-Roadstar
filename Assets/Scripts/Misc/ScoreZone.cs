using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreZone : MonoBehaviour
{
    private GameObject roundCanvas;
    private RoundManager roundManager;


    private void Start()
    {
        roundCanvas = GameObject.Find("RoundCanvas");
        roundManager = roundCanvas.GetComponent<RoundManager>();
        GetComponent<BoxCollider>().isTrigger = true;
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            roundManager.UpdateScore(other.GetComponent<PlayerInputs>().playerNum, other.GetComponent<CoinValueHeld>().coinValueHeld);
            other.GetComponent<CoinValueHeld>().coinValueHeld = 0;
            other.GetComponentInChildren<HUDManager>().UpdateHUD();
        }
    }
}
