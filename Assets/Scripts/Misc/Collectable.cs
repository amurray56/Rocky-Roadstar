﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic; //Needed to access lists
using UnityEngine.UI;


//class to add to collectible objects
[RequireComponent(typeof(SphereCollider))]
public class Collectable: MonoBehaviour
{
    //General
    public int coinValue = 20;
    private GameObject roundCanvas;

    private RoundManager roundManager;

    private Vector3 rotation = new Vector3(0, 80, 0); //Sets the rotation direction

    private void Start()
    {
        roundCanvas = GameObject.Find("RoundCanvas");
        roundManager = roundCanvas.GetComponent<RoundManager>();
        GetComponent<SphereCollider>().isTrigger = true;
    }

    private void Update()
    {
        transform.Rotate(rotation * Time.deltaTime, Space.World); //Rotates the object
    }

    private void OnTriggerEnter(Collider other) //Detects when the player enters the trigger
    {
        if(other.CompareTag("Player"))
        {
            roundManager.UpdateScore(other.GetComponent<PlayerInputs>().playerNum, coinValue);
            gameObject.SetActive(false);
        }
    }
}