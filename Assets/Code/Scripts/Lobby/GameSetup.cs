using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Realtime;
using System.Linq;

public class GameSetup : MonoBehaviourPunCallbacks
{
    public static GameSetup instance;

    [Header("Buttons")]
    public GameObject ReadyButton;
    public GameObject RedButton;
    public GameObject BlueButton;

    Image ReadyBtnImg;
    TextMeshProUGUI ReadyBtnText;
    Image RedBtnImg;
    TextMeshProUGUI RedBtnText;
    Image BlueBtnImg;
    TextMeshProUGUI BlueBtnText;

    ExitGames.Client.Photon.Hashtable m_properties = new ExitGames.Client.Photon.Hashtable();
    Team nextPlayerTeam;

    void Start()
    {
        instance = this;

        if (ReadyButton != null)
        {
            ReadyBtnText = ReadyButton.GetComponentInChildren<TextMeshProUGUI>();
            ReadyBtnImg = ReadyButton.GetComponent<Image>();
        }
        if(RedButton != null)
        {
            RedBtnImg = RedButton.GetComponent<Image>();
            RedBtnText = RedButton.GetComponentInChildren<TextMeshProUGUI>();
        }
        if (BlueButton != null)
        {
            BlueBtnImg = BlueButton.GetComponent<Image>();
            BlueBtnText = BlueButton.GetComponentInChildren<TextMeshProUGUI>();
        }
    }

    public void SetInitialPlayerSettings()
    {
        UpdateTeam();
        m_properties["Team"] = nextPlayerTeam;
        m_properties["Ready"] = false;
        PhotonNetwork.SetPlayerCustomProperties(m_properties);
    }

    //TODO: Algoritm balancing teams
    public void UpdateTeam()
    {
        if (nextPlayerTeam == Team.Red)
            nextPlayerTeam = Team.Blue;
        else
            nextPlayerTeam = Team.Red;
    }

    public void JoinRedTeam()
    {
        m_properties["Team"] = Team.Red;
        PhotonNetwork.SetPlayerCustomProperties(m_properties);
    }

    public void JoinBlueTeam()
    {
        m_properties["Team"] = Team.Blue;
        PhotonNetwork.SetPlayerCustomProperties(m_properties);
    }

    public void HandleReadyClick()
    {
        m_properties["Ready"] = !(bool)m_properties["Ready"];
        PhotonNetwork.SetPlayerCustomProperties(m_properties);
    }

    public void UpdateReadyButtonUI()
    {
        bool isReady = (bool)m_properties["Ready"];
        
        if (isReady)
        {
            ReadyBtnText.text = "UnReady";
            ReadyBtnImg.color = Color.white;
        }
        else
        {
            ReadyBtnText.text = "Ready";
            ReadyBtnImg.color = Color.green;
        }
    }

    public void UpdateTeamButtonsUI()
    {
        Team team = (Team)m_properties["Team"];

        switch (team)
        {
            case Team.Red:
                RedBtnImg.color = Color.white;
                RedBtnText.color = Color.black;
                BlueBtnImg.color = Color.blue;
                BlueBtnText.color = Color.white;
                break;
            case Team.Blue:
                RedBtnImg.color = Color.red;
                RedBtnText.color = Color.white;
                BlueBtnImg.color = Color.white;
                BlueBtnText.color = Color.black;
                break;
        }
    }

    public void UpdatePlayerCountText(TextMeshProUGUI label)
    {
        int playerNum = PhotonNetwork.CurrentRoom.PlayerCount;
        int roomSize = PhotonNetwork.CurrentRoom.MaxPlayers;
        label.text = $"{playerNum} / {roomSize}";
    }

    public bool checkIfTeamsBalanced()
    {
        int redCount = 0;
        int blueCount = 0;

        Player[] players = PhotonNetwork.PlayerList;
        foreach (Player player in players)
        {
            if (player.CustomProperties.ContainsKey("Team"))
            {
                Team team = (Team)player.CustomProperties["Team"];

                switch (team)
                {
                    case Team.Red:
                        redCount++;
                        break;
                    case Team.Blue:
                        blueCount++;
                        break;
                }
            }
        }

        if (redCount == blueCount)
            return true;
        else
        {
            Debug.Log("The number of players in Red and Blue teams must be equal to start a game.");
            return false;
        }
    }

    public bool checkIfAllPlayersReady()
    {
        Player[] players = PhotonNetwork.PlayerList;
        if (players.All(p => p.CustomProperties.ContainsKey("Ready") && (bool)p.CustomProperties["Ready"]))
        {
            return true;
        }
        Debug.Log("All players must be ready to play.");
        return false;
    }
}
