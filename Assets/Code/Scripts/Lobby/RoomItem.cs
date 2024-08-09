using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoomItem : MonoBehaviour
{
    public TextMeshProUGUI RoomName;
    public TextMeshProUGUI PlayerCount;

    private LobbyManager lobbyManager;

    private void Start()
    {
        lobbyManager = FindAnyObjectByType<LobbyManager>();
    }

    public void SetRoomName(string _roomName)
    {
        RoomName.text = _roomName;
    }

    public void JoinRoom()
    {
        lobbyManager.GetComponent<LobbyManager>().JoinRoomWithName(RoomName.text);
    }
}
