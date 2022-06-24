using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class RoundManager : MonoBehaviourPun
{
    //Scoreboard Settings
    public InputField p1Name;
    public InputField p2Name;
    public TextMeshProUGUI p1Score;
    public TextMeshProUGUI p2Score;
    public float runTimer = 0f;
    public GameObject canvas;

    //SINGLETON PATTERN
    public static RoundManager instance;

    public HUDManager hudManagerP1;
    public HUDManager hudManagerP2;

    private void Awake()
    {
        #region Singleton pattern
        if (!PhotonNetwork.IsConnected || PhotonNetwork.IsConnected && PhotonNetwork.IsMasterClient)
        {
            if (instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                instance = this;
            }
        }
        #endregion
    }

    //list of players
    public GameObject[] players;
    //list of scores
    public int[] playerScores;
    //list of spawn positions
    public Transform[] spawnPositions;

    public int maxKills;
    //list of scripts for the game manager to reference
    [SerializeField] RoundUIManager UIManager;

    public void Start()
    {
        if(PhotonNetwork.IsConnected)
        {
            Invoke("FindHUDOnline", .5f);
            Invoke("UpdateHUDOnline", .5f);
        }

        Invoke("FindHUD", 0.5f);
        Invoke("UpdateHUDManager", 0.5f);
        //Invoke("SetupScene", 0.1f);
    }

    [PunRPC]
    public void FindHUDOnline()
    {
        this.photonView.RPC("FindHUD", RpcTarget.All);
    }

    [PunRPC]
    public void UpdateHUDOnline()
    {
        this.photonView.RPC("UpdateHUDManager", RpcTarget.All);
    }

    //SETUP SCENE
    //check that all required scripts and prefabs are in the scene. Set up play area, and reset all variables for a new round
    //run the spawn players function for each player
    public void SetupScene()
    {
        if (!PhotonNetwork.IsConnected || PhotonNetwork.IsConnected && PhotonNetwork.IsMasterClient)
        {
            for (int i = 1; i < players.Length + 1; i++)
            {
                SpawnPlayer(i);
            }

            playerScores[0] = 0;
            playerScores[1] = 0;
            UIManager.UpdateScoreUI();

            if (PhotonNetwork.IsMasterClient)
            {
                UIManager.photonView.RPC("UpdateScoreUI", RpcTarget.All);
            }
        }
    }

    public void Update()
    {
        if (!PhotonNetwork.IsConnected || PhotonNetwork.IsMasterClient)
        {
            runTimer += Time.deltaTime;
        }
            
        if (!PhotonNetwork.IsConnected)
        {
            if (runTimer >= 120 && runTimer < 123)
                ZombieWarningOn();
            else if (runTimer >= 123 && runTimer < 124)
                ZombieWarningOff();
        }

        if (PhotonNetwork.IsMasterClient)
        {
            if (runTimer >= 120 && runTimer < 123)
                this.photonView.RPC("ZombieWarningOn", RpcTarget.All);
            else if (runTimer >= 123 && runTimer < 124)
                this.photonView.RPC("ZombieWarningOff", RpcTarget.All);
        }
    }

    [PunRPC]
    public void ZombieWarningOn()
    {
        canvas.SetActive(true);   
    }

    [PunRPC]
    public void ZombieWarningOff()
    {
        canvas.SetActive(false);
    }

    [PunRPC]
    public void EndRoundOnDeath()
    {
        if (!GameObject.Find("Player2(Clone)") && GameObject.Find("Player(Clone)").GetComponent<PlayerHealth>().numberOfLivesLeft <= 0 || !GameObject.FindGameObjectWithTag("Coin") && GameObject.Find("Player(Clone)").GetComponent<CoinValueHeld>().coinValueHeld == 0 && !GameObject.Find("Player2(Clone)"))
        {
            if (!PhotonNetwork.IsConnected)
                EndRound(1);

            if(PhotonNetwork.IsMasterClient)
                this.photonView.RPC("EndRound", RpcTarget.All, 1);

            if (PhotonNetwork.IsConnected)
            {
                p1Name.text = PhotonNetwork.LocalPlayer.Get(1).NickName;
                p1Score.text = playerScores[0].ToString();
            }
        }
        else if (GameObject.Find("Player(Clone)").GetComponent<PlayerHealth>().numberOfLivesLeft <= 0 && GameObject.Find("Player2(Clone)").GetComponent<PlayerHealth>().numberOfLivesLeft <= 0 || !GameObject.FindGameObjectWithTag("Coin") && GameObject.Find("Player(Clone)").GetComponent<CoinValueHeld>().coinValueHeld == 0 && GameObject.Find("Player2(Clone)").GetComponent<CoinValueHeld>().coinValueHeld == 0)
        {
            if (playerScores[0] == playerScores[1])
            {
                if (!PhotonNetwork.IsConnected)
                {
                    UIManager.UpdateScoreUI();
                    UIManager.DisplayResultsDraw();
                }

                if (PhotonNetwork.IsMasterClient)
                {
                    UIManager.photonView.RPC("UpdateScoreUI", RpcTarget.All);
                    UIManager.photonView.RPC("DisplayResultsDraw", RpcTarget.All);
                }

                if (PhotonNetwork.IsConnected)
                {
                    p1Name.text = PhotonNetwork.LocalPlayer.Get(1).NickName;
                    p2Name.text = PhotonNetwork.LocalPlayer.Get(GameObject.Find("Game Controller").GetComponent<GameController>().Player2ID).NickName;
                    p1Score.text = playerScores[0].ToString();
                    p2Score.text = playerScores[1].ToString();
                }
            }

            if (playerScores[0] > playerScores[1])
            {
                if (!PhotonNetwork.IsConnected)
                    EndRound(1);

                if (PhotonNetwork.IsMasterClient)
                    this.photonView.RPC("EndRound", RpcTarget.All, 1);

                if (PhotonNetwork.IsConnected)
                {
                    p1Name.text = PhotonNetwork.LocalPlayer.Get(1).NickName;
                    p2Name.text = PhotonNetwork.LocalPlayer.Get(GameObject.Find("Game Controller").GetComponent<GameController>().Player2ID).NickName;
                    p1Score.text = playerScores[0].ToString();
                    p2Score.text = playerScores[1].ToString();
                }
            }

            if (playerScores[0] < playerScores[1])
            {
                if (!PhotonNetwork.IsConnected)
                    EndRound(2);

                if (PhotonNetwork.IsMasterClient)
                    this.photonView.RPC("EndRound", RpcTarget.All, 2);

                if (PhotonNetwork.IsConnected)
                {
                    p1Name.text = PhotonNetwork.LocalPlayer.Get(1).NickName;
                    p2Name.text = PhotonNetwork.LocalPlayer.Get(GameObject.Find("Game Controller").GetComponent<GameController>().Player2ID).NickName;
                    p1Score.text = playerScores[0].ToString();
                    p2Score.text = playerScores[1].ToString();
                }
            }
        }
    }
    //SPAWN PLAYERS
    //takes in a player, finds a random position from the list, and spawns the player in that location.
    public void SpawnPlayer(int playerNumber)
    {
        if (!PhotonNetwork.IsConnected || PhotonNetwork.IsConnected && PhotonNetwork.IsMasterClient)
        {
            var player = Instantiate(players[playerNumber - 1], spawnPositions[playerNumber - 1].position, players[playerNumber - 1].transform.rotation);

            var playerInputs = player.GetComponent<PlayerInputs>();
            playerInputs.playerNum = playerNumber - 1;
            playerInputs.DetermineInputs();

            var playerHealth = player.GetComponent<PlayerHealth>();
            playerHealth.noDamage = false;
            playerHealth.Invoke("ResetDamage", 2f);
        }
    }
    //UPDATE SCORE
    // increments or decrements the score of a player, then checks all player scores to see if someone has won.
    // triggers the end round and passes information on. also calls out to the UI manager if it exists to update.
    [PunRPC]
    public void UpdateScore(int playerNum, int score)
    {
        playerScores[playerNum] += score;

        /*
        if(playerScores[playerNum] == winningScore)
        {
            Debug.Log("Player " + (playerNum + 1) + " Has Won");
            UIManager.UpdateScoreUI();
            EndRound(playerNum + 1);
        }
        */
    }

    [PunRPC]
    public void CheckForEnd()
    {
        if (!GameObject.FindGameObjectWithTag("Coin") && GameObject.Find("Player(Clone)").GetComponent<CoinValueHeld>().coinValueHeld == 0 && !GameObject.Find("Player2(Clone)"))
        {
            if(!PhotonNetwork.IsConnected)
                EndRound(1);

            if (PhotonNetwork.IsMasterClient)
                this.photonView.RPC("EndRound", RpcTarget.All, 1);

            if (PhotonNetwork.IsConnected)
                p1Name.text = PhotonNetwork.LocalPlayer.Get(1).NickName;
        }

        else if (!GameObject.FindGameObjectWithTag("Coin") && GameObject.Find("Player(Clone)").GetComponent<CoinValueHeld>().coinValueHeld == 0 && GameObject.Find("Player2(Clone)").GetComponent<CoinValueHeld>().coinValueHeld == 0)
        {
            if (playerScores[0] == playerScores[1])
            {
                if (!PhotonNetwork.IsConnected)
                {
                    UIManager.UpdateScoreUI();
                    UIManager.DisplayResultsDraw();
                }

                if(PhotonNetwork.IsMasterClient)
                {
                    UIManager.photonView.RPC("UpdateScoreUI", RpcTarget.All);
                    UIManager.photonView.RPC("DisplayResultsDraw", RpcTarget.All);
                }

                if (PhotonNetwork.IsConnected)
                {
                    p1Name.text = PhotonNetwork.LocalPlayer.Get(1).NickName;
                    p2Name.text = PhotonNetwork.LocalPlayer.Get(GameObject.Find("Game Controller").GetComponent<GameController>().Player2ID).NickName;
                    p1Score.text = playerScores[0].ToString();
                    p2Score.text = playerScores[1].ToString();
                }
            }

            else if (playerScores[0] > playerScores[1])
            {
                if (!PhotonNetwork.IsConnected)
                    EndRound(1);

                if (PhotonNetwork.IsMasterClient)
                    this.photonView.RPC("EndRound", RpcTarget.All, 1);

                if (PhotonNetwork.IsConnected)
                {
                    p1Name.text = PhotonNetwork.LocalPlayer.Get(1).NickName;
                    p2Name.text = PhotonNetwork.LocalPlayer.Get(GameObject.Find("Game Controller").GetComponent<GameController>().Player2ID).NickName;
                    p1Score.text = playerScores[0].ToString();
                    p2Score.text = playerScores[1].ToString();
                }
            }

            else if (playerScores[0] < playerScores[1])
            {
                if (!PhotonNetwork.IsConnected)
                    EndRound(2);

                if (PhotonNetwork.IsMasterClient)
                    this.photonView.RPC("EndRound", RpcTarget.All, 2);

                if (PhotonNetwork.IsConnected)
                {
                    p1Name.text = PhotonNetwork.LocalPlayer.Get(1).NickName;
                    p2Name.text = PhotonNetwork.LocalPlayer.Get(GameObject.Find("Game Controller").GetComponent<GameController>().Player2ID).NickName;
                    p1Score.text = playerScores[0].ToString();
                    p2Score.text = playerScores[1].ToString();
                }
            }
        }
    }
    //RUN TIMER
    //ticks down the timer and checks for end round. passes info to UI manager if it exists

    //END ROUND
    // calls an end to the round, triggers any end round events. Most likely this will pass of to another script/object that
    // handles score displays.

    [PunRPC]
    public void EndRound(int WinningPlayer)
    {
        Debug.Log("Game Over! Player " + WinningPlayer + " Has won the game!");
        if (UIManager != null)
        {
            if (!PhotonNetwork.IsConnected)
            {
                UIManager.DisplayResults(WinningPlayer);
                UIManager.UpdateScoreUI();
            }

            if(PhotonNetwork.IsMasterClient)
            {
                UIManager.photonView.RPC("DisplayResults", RpcTarget.All, WinningPlayer);
                UIManager.photonView.RPC("UpdateScoreUI", RpcTarget.All);
            }
        }
    }

    [PunRPC]
    public void FindHUD()
    {
        if (GameObject.Find("HUDP1") && GameObject.Find("HUDP2"))
        {
            hudManagerP1 = GameObject.Find("HUDP1").GetComponent<HUDManager>();
            hudManagerP2 = GameObject.Find("HUDP2").GetComponent<HUDManager>();
            p2Name.gameObject.SetActive(true);
            p2Score.gameObject.SetActive(true);
            UpdateHUDManager();
        }
        else if (GameObject.Find("HUDP1") && !GameObject.Find("HUDP2"))
        {
            hudManagerP1 = GameObject.Find("HUDP1").GetComponent<HUDManager>();
            p2Name.gameObject.SetActive(false);
            p2Score.gameObject.SetActive(false);
            UpdateHUDManager();
        }
    }

    [PunRPC]
    public void UpdateHUDManager()
    {
        if (GameObject.Find("HUDP1") && GameObject.Find("HUDP2"))
        {
            hudManagerP1 = GameObject.Find("HUDP1").GetComponent<HUDManager>();
            hudManagerP2 = GameObject.Find("HUDP2").GetComponent<HUDManager>();
            hudManagerP1.UpdateHUD();
            hudManagerP2.UpdateHUD();
        }
        else if (GameObject.Find("HUDP1") && !GameObject.Find("HUDP2"))
        {
            hudManagerP1 = GameObject.Find("HUDP1").GetComponent<HUDManager>();
            hudManagerP1.UpdateHUD();
        }
        else if (!GameObject.Find("HUDP1") && GameObject.Find("HUDP2"))
        {
            hudManagerP2 = GameObject.Find("HUDP2").GetComponent<HUDManager>();
            hudManagerP2.UpdateHUD();
        }
    }

    public void GameOver()
    {
        hudManagerP1.GameOverHUD();
        hudManagerP2.GameOverHUD();
    }
    public void LevelComplete()
    {
        hudManagerP1.LevelCompleteHUD();
        hudManagerP2.LevelCompleteHUD();
    }
}
/*
public void UpdateScore(int playerScoring, int playerKilled)
{
    if (playerScoring == 0 || playerScoring == playerKilled)
    {
        playerScores[playerKilled - 1]--;
    }
    else
    {
        playerScores[playerScoring - 1]++;
    }
    if (playerScores[playerScoring - 1] >= maxKills)
    {
        EndRound(playerScoring);
    }
    if (UIManager != null) UIManager.UpdateScoreUI();
}*/


