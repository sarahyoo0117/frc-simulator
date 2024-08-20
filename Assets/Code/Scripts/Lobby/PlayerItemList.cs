using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class PlayerItemList : MonoBehaviourPunCallbacks
{
    public PlayerItem PlayerItemPrefab;
    public Transform ParentContent;
    List<PlayerItem> PlayerItems = new List<PlayerItem>();

    public void UpdatePlayerList()
    {
        foreach(PlayerItem item in PlayerItems)
        {
            Destroy(item.gameObject);
        }
        PlayerItems.Clear();

        if (PhotonNetwork.CurrentRoom == null)
            return;

        foreach (KeyValuePair<int, Player> player in PhotonNetwork.CurrentRoom.Players)
        {
            PlayerItem newPlayerItem = Instantiate(PlayerItemPrefab, ParentContent);
            newPlayerItem.SetPlayerInfo(player.Value);

            if (player.Value == PhotonNetwork.LocalPlayer)
            {
                newPlayerItem.ApplyLocalChanges();
            }

            PlayerItems.Add(newPlayerItem);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdatePlayerList();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdatePlayerList();
    }
}
