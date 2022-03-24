using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
    public GameObject mainCanvas;
    public GameObject playCanvas;

    public void PlayGame()
    {
        mainCanvas.SetActive(false);
        playCanvas.SetActive(true);
    }

    public void SinglePlayerLevel1()
    {
        SceneManager.LoadScene(1);
    }
}
