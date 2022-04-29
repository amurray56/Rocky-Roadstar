using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinValueHeld : MonoBehaviour
{
    public int coinValueHeld;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Coin"))
        {
            coinValueHeld += other.GetComponent<Collectable>().coinValue;
            GetComponentInChildren<HUDManager>().UpdateHUD();
        }
    }
}
