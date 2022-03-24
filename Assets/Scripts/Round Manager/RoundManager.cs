using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    //SINGLETON PATTERN
    public static RoundManager instance;
    private void Awake()
    {
        #region Singleton pattern
        if(instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
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
        SetupScene();
    }
    //SETUP SCENE
    //check that all required scripts and prefabs are in the scene. Set up play area, and reset all variables for a new round
    //run the spawn players function for each player
    public void SetupScene()
    {
        for(int i = 1; i < players.Length+1; i++)
        {
            SpawnPlayer(i);
        }
        UIManager.UpdateScoreUI();
    }
    //SPAWN PLAYERS
    //takes in a player, finds a random position from the list, and spawns the player in that location.
    public void SpawnPlayer(int playerNumber)
    {
        var player = Instantiate(players[playerNumber-1], spawnPositions[playerNumber-1].position, players[playerNumber-1].transform.rotation);

        var playerInputs = player.GetComponent<PlayerInputs>();
        playerInputs.playerNum = playerNumber;
        playerInputs.DetermineInputs();

        var playerHealth = player.GetComponent<PlayerHealth>();
        playerHealth.noDamage = false;
        playerHealth.Invoke("ResetDamage", 2f);

    }
    //UPDATE SCORE
    // increments or decrements the score of a player, then checks all player scores to see if someone has won.
    // triggers the end round and passes information on. also calls out to the UI manager if it exists to update.
    public void UpdateScore(int playerScoring, int playerKilled)
    {
        if(playerScoring == 0 || playerScoring == playerKilled)
        {
            playerScores[playerKilled - 1]--;
        }
        else
        {
            playerScores[playerScoring - 1]++;
        }
        if(playerScores[playerScoring-1] >= maxKills)
        {
            EndRound(playerScoring);
        }
        if(UIManager != null) UIManager.UpdateScoreUI();
    }
    //RUN TIMER
    //ticks down the timer and checks for end round. passes info to UI manager if it exists

    //END ROUND
    // calls an end to the round, triggers any end round events. Most likely this will pass of to another script/object that
    // handles score displays.
    public void EndRound(int WinningPlayer)
    {
        Debug.Log("Game Over! Player " + WinningPlayer + " Has won the game!");
        if (UIManager != null) UIManager.DisplayResults(WinningPlayer);
    }
}
