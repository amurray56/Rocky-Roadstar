using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
  public void MainMenuButton()
    {
        GameObject.Find("Game Controller").GetComponent<GameController>().P1Name = GameObject.Find("RoundCanvas").GetComponent<RoundManager>().p1Name.text;
        GameObject.Find("Game Controller").GetComponent<GameController>().P2Name = GameObject.Find("RoundCanvas").GetComponent<RoundManager>().p2Name.text;
        GameObject.Find("Game Controller").GetComponent<GameController>().P1Score = GameObject.Find("RoundCanvas").GetComponent<RoundManager>().p1Score.text;
        GameObject.Find("Game Controller").GetComponent<GameController>().P2Score = GameObject.Find("RoundCanvas").GetComponent<RoundManager>().p2Score.text;

        PlayerPrefs.SetString("NameP1", GameObject.Find("RoundCanvas").GetComponent<RoundManager>().p1Name.text);
        PlayerPrefs.SetString("NameP2", GameObject.Find("RoundCanvas").GetComponent<RoundManager>().p2Name.text);
        PlayerPrefs.SetString("ScoreP1", GameObject.Find("RoundCanvas").GetComponent<RoundManager>().p1Score.text);
        PlayerPrefs.SetString("ScoreP2", GameObject.Find("RoundCanvas").GetComponent<RoundManager>().p2Score.text);

        GameData saveData = new GameData();
        GameObject.Find("Game Controller").GetComponent<SaveSystem>().SaveGame(saveData);
        SceneManager.LoadScene(0);
    }

    public void BackButton()
    {
        SceneManager.LoadScene(0);
    }
}
