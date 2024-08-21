using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.SceneManagement;

public enum Team : int
{
    Red = 0,
    Blue = 1
}

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public static LobbyManager instance;

    [Header("Current User Info")]
    public TextMeshProUGUI CurrentUsername;
    public TextMeshProUGUI SelectedRobotName;

    [Header("Pannels")]
    public GameObject HomePanel;
    public GameObject LobbyPanel;
    public GameObject progressLabel;

    [Header("Room Settings")]
    public int RoomSize = 6;
    public TMP_InputField RoomNameInput;
    public TextMeshProUGUI JoinedRoomName;
    public TextMeshProUGUI PlayerCount;
    [SerializeField]
    private PlayerItemList PlayerItemList;
    public GameObject StartButton;

    // bool isConnecting;

    private void Start()
    {
        instance = this;

        if (PhotonNetwork.IsConnected)
        {
            CurrentUsername.text = PhotonNetwork.NickName;
            PhotonNetwork.JoinLobby();
        }

        HomePanel.SetActive(true);
        progressLabel.SetActive(false);
        LobbyPanel.SetActive(false);
    }

    private void Update()
    {
        if (PhotonNetwork.IsMasterClient)
            StartButton.SetActive(true);
        else
            StartButton.SetActive(false);

        string robotName = PlayerPrefs.GetString("SelectedRobot");
        if (robotName != null)
            SelectedRobotName.text = robotName;
        else
            SelectedRobotName.text = "None";
    }

    public void CreateMyRoom()
    {
        if (RoomNameInput.text.Length >= 1)
            PhotonNetwork.CreateRoom(
                RoomNameInput.text,
                new RoomOptions { 
                    MaxPlayers = RoomSize,
                    BroadcastPropsChangeToAll = true
                });
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
        GameSetup.instance.UpdatePlayerCountText(PlayerCount);
        GameSetup.instance.SetInitialPlayerSettings();
        PlayerItemList.UpdatePlayerList();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        GameSetup.instance.UpdatePlayerCountText(PlayerCount);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        GameSetup.instance.UpdatePlayerCountText(PlayerCount);
    }

    public void OnClickStartButton()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (GameSetup.instance.checkIfAllPlayersReady() )
            {
                PhotonNetwork.LoadLevel("Game");
            }
        }
    }

    public void onClickChangeRobot()
    {
        SceneManager.LoadScene("RobotSelection");
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
        PhotonNetwork.CreateRoom(
            null,
            new RoomOptions {
                MaxPlayers = RoomSize,
                BroadcastPropsChangeToAll = true
            });
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to find a room: " + message);
    }
}
