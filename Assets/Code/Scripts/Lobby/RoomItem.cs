using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Pun;

public class RoomItem : MonoBehaviourPunCallbacks
{
    [Header("UI Settings")]
    public TextMeshProUGUI RoomName;
    public TextMeshProUGUI PlayerCount;

    public void SetRoomName(string _roomName)
    {
        RoomName.text = _roomName;
    }

    public void SetRoomPlayerCount(int _playerCount, int _maxPlayers)
    {
        PlayerCount.text = $"{_playerCount} / {_maxPlayers}";
    }

    public void JoinRoom()
    {
      LobbyManager.instance.JoinRoomWithName(RoomName.text);
    }
}
