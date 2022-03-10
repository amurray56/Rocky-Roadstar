using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    //Make this class accessible from any class - GameController.gameController.Variable or method name
    public static GameController gameController;

    //Setup
    public HUDManager hudManager;

    //Settings
    public int numberOfLivesLeft;
    public List<GameObject> enemies = new List<GameObject>();//List of enemies

    //General
    public bool gameComplete = false;
    public bool levelComplete = false;
    public bool restartLevel = false; // addition
    public int crystals, totalEnemiesKilled;

    public Vector3 playerRespawnPosition;
    public Vector3 playerCheckpointRespawn;
    //public bool gameMusic = true;

    //public AudioSource gameMusicAudioSource;

    void OnEnable()//addition
    {
        Debug.Log("OnEnable called");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)//addition
    {
        enemies.Clear();
        crystals = 0;
        totalEnemiesKilled = 0;
        if (restartLevel == true)
        {
            numberOfLivesLeft = 3;
            PlayerHealth.isDead = false;
            PlayerHealth.playerHealthAmount = 100f;
            restartLevel = false;
        }
        Debug.Log("OnSceneLoaded: " + scene.name);
        Debug.Log(mode);
        if (GameObject.Find("HUD"))
        {
            hudManager = GameObject.Find("HUD").GetComponent<HUDManager>();
            UpdateHUDManager();
        }
        //gameMusicAudioSource = GameObject.Find("GameMusic").GetComponent<AudioSource>();
        Invoke("UpdateHUDManager", 1f);
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
    void Start()
    {
        UpdateHUDManager();
    }

    private void Update()
    {
        if (numberOfLivesLeft == 0)
        {
            GameOver();
        }
    }
    public void UpdateHUDManager()
    {
        hudManager.UpdateHUD();
    }
    public void OpenHowToPlay()
    {
        //howToPlayPanelScript.ActivateHowToPlayPanel();
    }
    void PauseTheGame()
    {
        Time.timeScale = 0.1f;
    }
    /*
    public bool GameMusicToggle()
    {
        if (!gameMusicAudioSource)
        {
            Debug.LogWarning("No Audio Source for game music found");
            return false;
        }
        if (gameMusic && gameMusicAudioSource.isPlaying)
        {
            gameMusic = false;
            gameMusicAudioSource.Stop();
            return false;
        }
        else
        {
            gameMusic = true;
            gameMusicAudioSource.Play();
            return true;
        }
    }
    */
    public void GameOver()
    {
        hudManager.GameOverHUD();
        restartLevel = true;
    }
    public void LevelComplete()
    {
        hudManager.LevelCompleteHUD();
    }
}
