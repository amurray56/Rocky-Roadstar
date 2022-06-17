using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class RoundUIManager : MonoBehaviour
{
    [SerializeField] TMP_Text[] playerScoreUis;
    [SerializeField] CanvasGroup WinScreen;
    [SerializeField] TMP_Text winningPlayerName;

    public void UpdateScoreUI()
    {
        if (!PhotonNetwork.IsConnected || PhotonNetwork.IsConnected && PhotonNetwork.IsMasterClient)
        {
            for (int i = 0; i < playerScoreUis.Length; i++)
            {
                playerScoreUis[i].text = RoundManager.instance.playerScores[i].ToString();

                //Orignal line
                //playerScoreUis[i].text = "Player " + (i + 1).ToString() + " : " + RoundManager.instance.playerScores[i].ToString();
            }
        }
    }
    public void DisplayResults(int winningPlayer)
    {
        if (!PhotonNetwork.IsConnected || PhotonNetwork.IsConnected && PhotonNetwork.IsMasterClient)
        {
            winningPlayerName.text = "Player " + (winningPlayer).ToString();
            WinScreen.gameObject.SetActive(true);
            StartCoroutine(CanvasFadeIn());
        }
    }

    public void DisplayResultsDraw()
    {
        if (!PhotonNetwork.IsConnected || PhotonNetwork.IsConnected && PhotonNetwork.IsMasterClient)
        {
            winningPlayerName.text = "Draw";
            WinScreen.gameObject.SetActive(true);
            StartCoroutine(CanvasFadeIn());
        }
    }

    IEnumerator CanvasFadeIn()
    {
        if (!PhotonNetwork.IsConnected || PhotonNetwork.IsConnected && PhotonNetwork.IsMasterClient)
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
}
