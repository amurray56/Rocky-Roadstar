using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scoreboard : MonoBehaviour
{
    public Text playerName1;
    public Text playerName2;
    public Text playerScore1;
    public Text playerScore2;

    // Start is called before the first frame update
    void Start()
    {
        GameObject.Find("Game Controller").GetComponent<SaveSystem>().LoadGame();

        playerName1.text = PlayerPrefs.GetString("NameP1");
        playerName2.text = PlayerPrefs.GetString("NameP2");
        playerScore1.text = PlayerPrefs.GetString("ScoreP1");
        playerScore2.text = PlayerPrefs.GetString("ScoreP2");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
