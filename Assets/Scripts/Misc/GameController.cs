using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    //Make this class accessible from any class - GameController.gameController.Variable or method name
    public static GameController gameController;

    //Player Names & Scores
    public string P1Name;
    public string P2Name;
    public string P1Score;
    public string P2Score;

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

        P1Name = null;
        P2Name = null;
        P1Score = null;
        P2Score = null;
    }

    void PauseTheGame()
    {
        Time.timeScale = 0.1f;
    }
}
