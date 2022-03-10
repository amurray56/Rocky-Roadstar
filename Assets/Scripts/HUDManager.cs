using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    //Setup
    public GameObject hud; //Access the HUD and the animator
    public GameObject gameOver;
    public GameObject victoryPanel;
    public GameObject pausePanel;
    public static bool gamePaused;
    public static bool victory;
    public static bool lose;
    
    //Settings
    public Text HUDLives;
    public Text HUDCrystals;
    public Slider HUDHealthSlider; //Allos access to GUI health slider
    public Text HUDEnemiesKilled;

    public void Awake()
    {
        Time.timeScale = 1;
        PlayerHealth.isDead = false;
        victory = false;
        lose = false;
        gamePaused = false;
        gameOver.SetActive(false);
        hud.SetActive(true);
        victoryPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Update()
    {

        if (GameController.gameController.numberOfLivesLeft > 0 && victoryPanel.activeSelf == false)
        {
            gameOver.SetActive(false);
            Time.timeScale = 1;
            hud.SetActive(true);
            Cursor.lockState = CursorLockMode.Locked;
        }
        if (victoryPanel.activeSelf == true)
        {
            hud.SetActive(false);
            //Time.timeScale = 0;
            gamePaused = true;
            Cursor.lockState = CursorLockMode.None;
            victory = true;
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
        if (Input.GetKeyDown(KeyCode.Escape) && pausePanel.activeSelf == false)
        {
            Time.timeScale = 0;
            pausePanel.SetActive(true);
            gamePaused = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && pausePanel.activeSelf == true || pausePanel.activeSelf == false)
        {
            Time.timeScale = 1;
            pausePanel.SetActive(false);
            gamePaused = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        if (gameOver.activeSelf || victoryPanel.activeSelf || pausePanel.activeSelf)
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void UpdateHUD()
    {
        HUDLives.text = GameController.gameController.numberOfLivesLeft.ToString();
        HUDCrystals.text = GameController.gameController.crystals.ToString();
        HUDHealthSlider.value = PlayerHealth.playerHealthAmount;
        HUDEnemiesKilled.text = GameController.gameController.totalEnemiesKilled.ToString();
        //playerHealth is a static variable so we can access it from the script name and then the variable
    }

    public void GameOverHUD()
    {
        if (GameController.gameController.numberOfLivesLeft == 0)
        {
            gamePaused = true;
            hud.SetActive(false);
            gameOver.SetActive(true);
            //Time.timeScale = 0;
            lose = true;
        }
    }

    public void LevelCompleteHUD()
    {

    }
}
