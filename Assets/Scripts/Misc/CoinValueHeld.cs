using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinValueHeld : MonoBehaviour
{
    [HideInInspector]
    public int coinValueHeld;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Coin"))
        {
            coinValueHeld += other.GetComponent<Collectable>().coinValue;
            GetComponentInChildren<HUDManager>().UpdateHUD();
            other.gameObject.SetActive(false);
        }
    }
}
