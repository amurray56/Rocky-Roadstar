using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
    public GameObject playButton, backButton, fleeButton, multiplayerButton, selectLevelImage, soloLevel1Button, title, coopButton;
    private RoundManager roundManager;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void SelectLevelSolo()
    {
        playButton.SetActive(false);
        soloLevel1Button.SetActive(true);
        multiplayerButton.SetActive(false);
        fleeButton.SetActive(false);
        backButton.SetActive(true);
        title.SetActive(false);
        selectLevelImage.SetActive(true);
        coopButton.SetActive(false);
    }

    public void SelectLevelMultiplayer()
    {
        playButton.SetActive(false);
        soloLevel1Button.SetActive(false);
        multiplayerButton.SetActive(false);
        fleeButton.SetActive(false);
        backButton.SetActive(true);
        title.SetActive(false);
        selectLevelImage.SetActive(true);
        coopButton.SetActive(true);
    }

    public void BackButton()
    {
        playButton.SetActive(true);
        soloLevel1Button.SetActive(false);
        multiplayerButton.SetActive(true);
        fleeButton.SetActive(true);
        backButton.SetActive(false);
        title.SetActive(true);
        selectLevelImage.SetActive(false);
        coopButton.SetActive(false);
    }

    public void SinglePlayerLevel1()
    {
        SceneManager.LoadScene(1);
        Invoke("LoadOnePlayer", .1f);
        selectLevelImage.SetActive(false);
        backButton.SetActive(false);
        soloLevel1Button.SetActive(false);
    }
    public void CoopLevel1()
    {
        SceneManager.LoadScene(1);
        Invoke("LoadTwoPlayer", .1f);
        selectLevelImage.SetActive(false);
        backButton.SetActive(false);
        coopButton.SetActive(false);
    }

    public void LoadOnePlayer()
    {
        roundManager = GameObject.Find("RoundCanvas").GetComponent<RoundManager>();
        roundManager.SpawnPlayer(1);
        GameObject.Find("Player(Clone)").GetComponentInChildren<Camera>().rect = new Rect(0, 0, 1, 1);
    }

    public void LoadTwoPlayer()
    {
        roundManager = GameObject.Find("RoundCanvas").GetComponent<RoundManager>();
        roundManager.SetupScene();
        GameObject.Find("Player(Clone)").GetComponentInChildren<Camera>().rect = new Rect(0, 0, .5f, 1);
        GameObject.Find("Player2(Clone)").GetComponentInChildren<Camera>().rect = new Rect(0.5f, 0, .5f, 1);
    }
}
