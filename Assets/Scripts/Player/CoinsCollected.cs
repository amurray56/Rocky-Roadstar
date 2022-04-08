using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinsCollected : MonoBehaviour
{
    public int coins = 0;
    public int coinValue = 20;
    private RoundManager roundManager;

    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Coin"))
        {
            coins += coinValue;
            roundManager.UpdateScore(coinValue, 0);
        }
    }
}
