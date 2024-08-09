using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class RoomItemList : MonoBehaviourPunCallbacks
{
    public RoomItem RoomItemPrefab;
    public Transform ParentContent;

    public float timeBetweenUpdates = 1.5f;
    float nextUpdateTime;

    List<RoomItem> roomItems = new List<RoomItem>();

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        if (Time.time >= nextUpdateTime)
        {
            UpdateRoomList(roomList);
            nextUpdateTime = Time.time + timeBetweenUpdates;
        }
    }

    public void UpdateRoomList(List<RoomInfo> roomList)
    {
        foreach (RoomItem roomItem in roomItems)
        {
            Destroy(roomItem.gameObject);
        }
        roomItems.Clear();

        foreach (RoomInfo room in roomList)
        {
            RoomItem newRoomItem = Instantiate(RoomItemPrefab, ParentContent);
            newRoomItem.SetRoomName(room.Name);
            roomItems.Add(newRoomItem);
        }
    }
}
