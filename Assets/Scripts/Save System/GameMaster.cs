using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    GameData saveData = new GameData();

    SaveSystem saveGame;

    private void Awake()
    {
        saveGame = GetComponent<SaveSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            saveData.AddScore(1);
            PrintScore();
            Debug.Log(saveData);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            saveData.AddScore(-1);
            PrintScore();
            Debug.Log(saveData);
        }
        if (Input.GetKeyDown(KeyCode.F1))
        {
            saveGame.SaveGame(saveData);
            Debug.Log("Saved data.");
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            saveGame.LoadGame();
            Debug.Log("Loaded data");
            PrintScore();
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            saveData.ResetData();
            PrintScore();
            Debug.Log(saveData);
        }
    }

    void PrintScore()
    {
        Debug.Log("The current score is " + saveData.score);
    }
}
