using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    //Make this class accessible from any class - GameController.gameController.Variable or method name
    public static GameController gameController;

    //Setup
    public HUDManager hudManagerP1;
    public HUDManager hudManagerP2;

    //Settings
    public List<GameObject> enemies = new List<GameObject>();//List of enemies

    //General
    public bool gameComplete = false;
    public bool levelComplete = false;
    public bool restartLevel = false; // addition

    void OnEnable()//addition
    {
        Debug.Log("OnEnable called");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)//addition
    {
        enemies.Clear();
  
        Debug.Log("OnSceneLoaded: " + scene.name);
        Debug.Log(mode);
        Invoke("FindHUD", 1f);
        Invoke("UpdateHUDManager", 1.1f);
    }

    private void FindHUD()
    {
        if (GameObject.Find("HUDP1") && GameObject.Find("HUDP2"))
        {
            hudManagerP1 = GameObject.Find("HUDP1").GetComponent<HUDManager>();
            hudManagerP2 = GameObject.Find("HUDP2").GetComponent<HUDManager>();
            UpdateHUDManager();
        }
        else
        {
            hudManagerP1 = GameObject.Find("HUDP1").GetComponent<HUDManager>();
            UpdateHUDManager();
        }
    }
    private void Awake()
    {
        if (gameController == null)
        {
            DontDestroyOnLoad(gameObject);
            gameController = this;
        }
        else if (gameController != null)
        {
            Destroy(gameObject);
        }
    }

    public void UpdateHUDManager()
    {
        if (GameObject.Find("HUDP2"))
        {
            hudManagerP1.UpdateHUD();
            hudManagerP2.UpdateHUD();
        }
        else
        {
            hudManagerP1.UpdateHUD();
        }
    }
    void PauseTheGame()
    {
        Time.timeScale = 0.1f;
    }
    public void GameOver()
    {
        hudManagerP1.GameOverHUD();
        hudManagerP2.GameOverHUD();
        restartLevel = true;
    }
    public void LevelComplete()
    {
        hudManagerP1.LevelCompleteHUD();
        hudManagerP2.LevelCompleteHUD();
    }
}
