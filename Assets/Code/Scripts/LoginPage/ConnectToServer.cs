using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.SceneManagement;

public class ConnectToServer : MonoBehaviourPunCallbacks
{
    string gameVersion = "1"; //Users are separated by the game version.

    public TMP_InputField UsernameInput;
    public TextMeshProUGUI ButtonText;

    public void Connect()
    {
        if (UsernameInput.text.Length >= 1)
        {
            PhotonNetwork.NickName = UsernameInput.text;
            ButtonText.text = "Connecting...";

            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = gameVersion;
        }
    }

    public override void OnConnectedToMaster()
    {
        SceneManager.LoadScene("Lobby");
    }
}
