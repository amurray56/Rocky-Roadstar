using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using UnityEngine.SceneManagement;
using Photon.Realtime;

public class Lobby : MonoBehaviourPunCallbacks
{
    public TextMeshProUGUI hostName;
    public TextMeshProUGUI player2Name;

    public TextMeshProUGUI noPlayer2;

    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        hostName.text = PhotonNetwork.LocalPlayer.Get(1).NickName;
        if (!PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.AutomaticallySyncScene = true;
            player2Name.text = PhotonNetwork.LocalPlayer.NickName;
            GameObject.Find("Game Controller").GetComponent<GameController>().Player2ID = PhotonNetwork.LocalPlayer.ActorNumber;
        }
    }

    private void Update()
    {
        if(noPlayer2.gameObject.activeSelf)
        {
            timer += Time.deltaTime;
        }

        if (timer >= 2)
            noPlayer2.gameObject.SetActive(false);

        //if(PhotonNetwork.CurrentRoom.PlayerCount == 2)
        //{
            //hostName.text = PhotonNetwork.LocalPlayer.Get(1).NickName;
            //player2Name.text = PhotonNetwork.LocalPlayer.Get(2).NickName;
        //}
    }

    public void StartGame()
    {
        if (PhotonNetwork.IsMasterClient && !string.IsNullOrEmpty(player2Name.text))
            PhotonNetwork.LoadLevel(2);
        else if (string.IsNullOrEmpty(player2Name.text))
        {
            Debug.Log("No Player 2");
            timer = 0;
            noPlayer2.gameObject.SetActive(true);
        }
    }

    public void BackButton()
    {
        PhotonNetwork.LeaveRoom();
        if (PhotonNetwork.IsConnected)
            PhotonNetwork.Disconnect();
        SceneManager.LoadScene(0);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        player2Name.text = null;
        
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        hostName.text = PhotonNetwork.LocalPlayer.Get(1).NickName;
        player2Name.text = newPlayer.NickName;
        GameObject.Find("Game Controller").GetComponent<GameController>().Player2ID = newPlayer.ActorNumber;
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        base.OnMasterClientSwitched(newMasterClient);
        PhotonNetwork.LeaveRoom();
        if (PhotonNetwork.IsConnected)
            PhotonNetwork.Disconnect();
        SceneManager.LoadScene(0);
    }
}
