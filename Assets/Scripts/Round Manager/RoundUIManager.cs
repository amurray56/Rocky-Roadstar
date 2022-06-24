using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class RoundUIManager : MonoBehaviourPun
{
    [SerializeField] TMP_Text[] playerScoreUis;
    [SerializeField] CanvasGroup WinScreen;
    [SerializeField] TMP_Text winningPlayerName;

    [PunRPC]
    public void UpdateScoreUI()
    {
        for (int i = 0; i < playerScoreUis.Length; i++)
        {
            playerScoreUis[i].text = gameObject.GetComponent<RoundManager>().playerScores[i].ToString();

            //Orignal line
            //playerScoreUis[i].text = "Player " + (i + 1).ToString() + " : " + RoundManager.instance.playerScores[i].ToString();
        }
    }

    [PunRPC]
    public void DisplayResults(int winningPlayer)
    {
        winningPlayerName.text = "Player " + (winningPlayer).ToString();
        WinScreen.gameObject.SetActive(true);
        StartCoroutine(CanvasFadeIn());
    }

    [PunRPC]
    public void DisplayResultsDraw()
    {
        winningPlayerName.text = "Draw";
        WinScreen.gameObject.SetActive(true);
        StartCoroutine(CanvasFadeIn());
    }

    [PunRPC]
    IEnumerator CanvasFadeIn()
    {
        WaitForEndOfFrame WFEOF = new WaitForEndOfFrame();
        while (WinScreen.alpha < 0.99)
        {
            WinScreen.alpha = Mathf.Lerp(WinScreen.alpha, 1, 0.01f);
            yield return WFEOF;
        }
        yield return null;
    }
}
