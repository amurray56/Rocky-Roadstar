using System.Collections.Generic;
using UnityEngine;
public class HighScores : MonoBehaviour
{
    public HighScoreDisplay[] highScoreDisplayArray;
    List<HighScoreEntry> scores = new List<HighScoreEntry>();

    private GameObject gameControllerObj;
    private GameController gameController;

    void Start()
    {
        gameControllerObj = GameObject.Find("Game Controller");
        gameController = gameControllerObj.GetComponent<GameController>();

        Load();
        
        if(gameController.P1Name != null && gameController.P1Score != null)
        {
            AddNewScore(gameController.P1Name, int.Parse(gameController.P1Score));
            Save();
            Debug.Log("P1 Score Saved");
            gameController.P1Name = null;
            gameController.P1Score = null;
        }
        
        if(gameController.P2Name != null && gameController.P2Score != null)
        {
            AddNewScore(gameController.P2Name, int.Parse(gameController.P2Score));
            Save();
            Debug.Log("P2 Score Saved");
            gameController.P2Name = null;
            gameController.P2Score = null;
        }
        
        UpdateDisplay();
    }
    void UpdateDisplay()
    {
        scores.Sort((HighScoreEntry x, HighScoreEntry y) => y.score.CompareTo(x.score));
        for (int i = 0; i < highScoreDisplayArray.Length; i++)
        {
            if (i < scores.Count)
            {
                highScoreDisplayArray[i].DisplayHighScore(scores[i].name, scores[i].score);
            }
            else
            {
                highScoreDisplayArray[i].HideEntryDisplay();
            }
        }
    }
    void AddNewScore(string entryName, int entryScore)
    {
        scores.Add(new HighScoreEntry { name = entryName, score = entryScore });
    }

    void Save()
    {
        XMLManager.instance.SaveScores(scores);
    }

    void Load()
    {
        scores = XMLManager.instance.LoadScores();
    }
}
