using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;

public class PlayerItem : MonoBehaviourPunCallbacks
{
    [Header("UI Settings")]
    public TextMeshProUGUI playerName;
    public GameObject hostLabel;
    public TextMeshProUGUI alliance;
    public TextMeshProUGUI ready;
    public Image backgroundImage;

    [Header("Custom Properties")]
    public Team myTeam;
    public bool isReady;

    Player player;

    public void SetPlayerInfo(Player _player)
    {
        player = _player;
        playerName.text = _player.NickName;
        UpdatePlayerItem(player);

        //TODO: transfer host
        if (player == PhotonNetwork.MasterClient)
            hostLabel.SetActive(true);
        else
            hostLabel.SetActive(false);
    }

    public void ApplyLocalChanges()
    {
        backgroundImage.color = Color.yellow;
    }

    public void UpdateReadyText()
    {
        ready.text = isReady ? "Ready" : "Not Ready";
    }

    public void UpdateAllianceText()
    {
        if (myTeam == Team.Red)
        {
            alliance.text = "Red";
            alliance.color = Color.red;
        }
        else
        {
            alliance.text = "Blue";
            alliance.color = Color.blue;
        }
    }

    public void UpdatePlayerItem(Player player)
    {
        if (player.CustomProperties.ContainsKey("Team"))
        {
            myTeam = (Team)player.CustomProperties["Team"];
            UpdateAllianceText();
            GameSetup.instance.UpdateTeamButtonsUI();
        }
        if (player.CustomProperties.ContainsKey("Ready"))
        {
            isReady = (bool)player.CustomProperties["Ready"];
            UpdateReadyText();
            GameSetup.instance.UpdateReadyButtonUI();
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (player == targetPlayer)
        {
            UpdatePlayerItem(targetPlayer);
        }
    }
}
