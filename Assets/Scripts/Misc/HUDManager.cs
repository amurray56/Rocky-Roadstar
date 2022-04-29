using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    //Setup
    public GameObject hud; //Access the HUD and the animator
    public GameObject gameOver;
    public static bool gamePaused;
    public static bool victory;
    public static bool lose;
    private int playerNum;
    
    //Settings
    public Text HUDLives;
    public Text HUDScore;
    public Text HUDCoinsHeld;
    public Slider HUDHealthSlider; //Allos access to GUI health slider
    //public Text HUDEnemiesKilled;

    public void Awake()
    {
        //Time.timeScale = 1;
        //PlayerHealth.isDead = false;
        victory = false;
        lose = false;
        gamePaused = false;
        gameOver.SetActive(false);
        hud.SetActive(true);
        playerNum = GetComponentInParent<PlayerInputs>().playerNum;
        //Cursor.lockState = CursorLockMode.Locked;
    }

    public void Update()
    {

        if (GetComponentInParent<PlayerHealth>().numberOfLivesLeft > 0)
        {
            gameOver.SetActive(false);
            //Time.timeScale = 1;
            hud.SetActive(true);
            //Cursor.lockState = CursorLockMode.Locked;
        }
        if (gameOver.activeSelf == true)
        {
            gamePaused = true;
            hud.SetActive(false);
            gameOver.SetActive(true);
            //Time.timeScale = 0;
            lose = true;
            Cursor.lockState = CursorLockMode.None;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //Time.timeScale = 0;
            gamePaused = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            //Time.timeScale = 1;
            gamePaused = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        if (gameOver.activeSelf)
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void UpdateHUD()
    {
        HUDLives.text = GetComponentInParent<PlayerHealth>().numberOfLivesLeft.ToString();
        HUDHealthSlider.value = GetComponentInParent<PlayerHealth>().playerHealthAmount;
        HUDScore.text = GameObject.Find("RoundCanvas").GetComponent<RoundManager>().playerScores[playerNum].ToString();
        HUDCoinsHeld.text = GetComponentInParent<CoinValueHeld>().coinValueHeld.ToString();
    }

    public void GameOverHUD()
    {
        if (GetComponentInParent<PlayerHealth>().numberOfLivesLeft == 0)
        {
            gamePaused = true;
            hud.SetActive(false);
            gameOver.SetActive(true);
            Time.timeScale = 0;
            lose = true;
        }
    }

    public void LevelCompleteHUD()
    {

    }
}
