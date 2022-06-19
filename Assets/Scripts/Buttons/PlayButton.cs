using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class PlayButton : MonoBehaviour
{
    public GameObject playButton, backButton, fleeButton, multiplayerButton, selectLevelImage, soloLevel1Button, title, coopButton, scoreBoard, onlineButton, controlPanel, backToMultiplayer;
    private RoundManager roundManager;
    private PlayButton playButtonScript;

    private void Awake()
    {
        if (!PhotonNetwork.IsConnected || PhotonNetwork.IsConnected && PhotonNetwork.IsMasterClient)
        {
            if (playButtonScript != null)
            {
                Destroy(gameObject);
            }
            else
            {
                playButtonScript = this;
            }
        }
    }

    public void ScoreBoard()
    {
        SceneManager.LoadScene(2);
    }

    public void SelectLevelSolo()
    {
        playButton.SetActive(false);
        soloLevel1Button.SetActive(true);
        multiplayerButton.SetActive(false);
        fleeButton.SetActive(false);
        scoreBoard.SetActive(false);
        backButton.SetActive(true);
        title.SetActive(false);
        selectLevelImage.SetActive(true);
        coopButton.SetActive(false);
        onlineButton.SetActive(false);
    }

    public void SelectLevelMultiplayer()
    {
        playButton.SetActive(false);
        soloLevel1Button.SetActive(false);
        multiplayerButton.SetActive(false);
        fleeButton.SetActive(false);
        scoreBoard.SetActive(false);
        backButton.SetActive(true);
        title.SetActive(false);
        selectLevelImage.SetActive(true);
        coopButton.SetActive(true);
        onlineButton.SetActive(true);
        controlPanel.SetActive(false);
        backToMultiplayer.SetActive(false);
    }

    public void Online()
    {
        playButton.SetActive(false);
        soloLevel1Button.SetActive(false);
        multiplayerButton.SetActive(false);
        fleeButton.SetActive(false);
        scoreBoard.SetActive(false);
        backButton.SetActive(false);
        title.SetActive(false);
        selectLevelImage.SetActive(false);
        coopButton.SetActive(false);
        onlineButton.SetActive(false);
        controlPanel.SetActive(true);
        backToMultiplayer.SetActive(true);
    }

    public void BackButton()
    {
        playButton.SetActive(true);
        soloLevel1Button.SetActive(false);
        multiplayerButton.SetActive(true);
        fleeButton.SetActive(true);
        scoreBoard.SetActive(true);
        backButton.SetActive(false);
        title.SetActive(true);
        selectLevelImage.SetActive(false);
        coopButton.SetActive(false);
        onlineButton.SetActive(false);
    }

    public void SinglePlayerLevel1()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.LoadScene(1);
        Invoke("LoadOnePlayer", .1f);
        selectLevelImage.SetActive(false);
        backButton.SetActive(false);
        soloLevel1Button.SetActive(false);
    }
    public void CoopLevel1()
    {
        DontDestroyOnLoad(gameObject);
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
        Invoke("DestroyCanvas", 1f);
    }

    public void LoadTwoPlayer()
    {
        roundManager = GameObject.Find("RoundCanvas").GetComponent<RoundManager>();
        roundManager.SetupScene();
        GameObject.Find("Player(Clone)").GetComponentInChildren<Camera>().rect = new Rect(0, 0, .5f, 1);
        GameObject.Find("Player2(Clone)").GetComponentInChildren<Camera>().rect = new Rect(0.5f, 0, .5f, 1);
        Invoke("DestroyCanvas", 1f);
    }

    public void DestroyCanvas()
    {
        Destroy(gameObject);
    }
}
