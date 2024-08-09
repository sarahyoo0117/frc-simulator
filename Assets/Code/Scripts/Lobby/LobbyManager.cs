using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public int RoomSize = 6;

    [Header("Current User Info")]
    public TextMeshProUGUI CurrentUsername;

    [Header("Pannels")]
    public GameObject HomePanel;
    public GameObject LobbyPanel;
    public GameObject progressLabel;

    [Header("Custom Room Settings")]
    public TMP_InputField RoomNameInput;
    public TextMeshProUGUI JoinedRoomName;


   // bool isConnecting;

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    private void Start()
    {
        if (PhotonNetwork.IsConnected)
        {
            CurrentUsername.text = PhotonNetwork.NickName;
            PhotonNetwork.JoinLobby();
        }

        HomePanel.SetActive(true);
        progressLabel.SetActive(false);
        LobbyPanel.SetActive(false);
    }

    public void CreateMyRoom()
    {
        if (RoomNameInput.text.Length >= 1)
            PhotonNetwork.CreateRoom(RoomNameInput.text, new RoomOptions { MaxPlayers = RoomSize });
    }

    public void QuickMatch()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public void JoinRoomWithName(string RoomName)
    {
        PhotonNetwork.JoinRoom(RoomName);
    }

    public void ExitRoom()
    {
        //TODO: How to destroy room or roomItem if player count is 0?
        if (PhotonNetwork.CurrentRoom.PlayerCount == 0)
            PhotonNetwork.CurrentRoom.IsVisible = false;
        PhotonNetwork.LeaveRoom();
    }

    public override void OnJoinedRoom()
    {
        HomePanel.SetActive(false);
        LobbyPanel.SetActive(true);
        JoinedRoomName.text = PhotonNetwork.CurrentRoom.Name;
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnLeftRoom()
    {
        LobbyPanel.SetActive(false);
        HomePanel.SetActive(true);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to find a room. Trying to create a new room...");

        //TODO: RandomRoom name
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = RoomSize });
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to find a room: " + message);
    }
}
