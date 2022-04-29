using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LivesRemaining : MonoBehaviour
{
    public Image[] livesP1;
    public Image[] livesP2;


    private GameObject playerOne;
    private GameObject playerTwo;
    private PlayerHealth livesLeftP1;
    private PlayerHealth livesLeftP2;

    private void Awake()
    {
        playerOne = GameObject.Find("PlayerOne");
        playerTwo = GameObject.Find("PlayerTwo");
        livesLeftP1 = playerOne.GetComponent<PlayerHealth>();
        livesLeftP2 = playerTwo.GetComponent<PlayerHealth>();
    }

    public void LifeLost()
    {
        if(livesLeftP1.numberOfLivesLeft == 2)
        {
            livesP1[2].enabled = true;
            livesP1[1].enabled = false;
            livesP1[0].enabled = false;
        }

        if (livesLeftP1.numberOfLivesLeft == 1)
        {
            livesP1[2].enabled = false;
            livesP1[1].enabled = true;
            livesP1[0].enabled = false;
        }

        if (livesLeftP1.numberOfLivesLeft == 0)
        {
            livesP1[2].enabled = false;
            livesP1[1].enabled = false;
            livesP1[0].enabled = true;
        }

        if (livesLeftP2.numberOfLivesLeft == 2)
        {
            livesP2[2].enabled = true;
            livesP2[1].enabled = false;
            livesP2[0].enabled = false;
        }

        if (livesLeftP2.numberOfLivesLeft == 1)
        {
            livesP2[2].enabled = false;
            livesP2[1].enabled = true;
            livesP2[0].enabled = false;
        }

        if (livesLeftP2.numberOfLivesLeft == 0)
        {
            livesP2[2].enabled = false;
            livesP2[1].enabled = false;
            livesP2[0].enabled = true;
        }
    }
}
