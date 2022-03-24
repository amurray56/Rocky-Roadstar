using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerButton : MonoBehaviour
{
    public GameObject playCanvas;
    public GameObject mainMenuCanvas;
    public GameObject multiplayerCanvas;

    public void Multiplayer()
    {
        mainMenuCanvas.SetActive(false);
        playCanvas.SetActive(false);
        multiplayerCanvas.SetActive(true);
    }
}
