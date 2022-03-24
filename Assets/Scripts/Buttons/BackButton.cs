using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackButton : MonoBehaviour
{
    public GameObject mainCanvas;
    public GameObject playCanvas;
    public GameObject multiplayerCanvas;

    public void Back()
    {
        playCanvas.SetActive(false);
        multiplayerCanvas.SetActive(false);
        mainCanvas.SetActive(true);
    }
}
