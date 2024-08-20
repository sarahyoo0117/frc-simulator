using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks
{
    public GameObject RedPlayerPrefab;
    public GameObject BluePlayerPrefab;

    void Start()
    {
        if (PlayerManager.LocalPlayerInstance == null)
        {
            Team team = (Team) PhotonNetwork.LocalPlayer.CustomProperties["Team"];
            StartCoroutine(InstantiatePlayer(2, team));        
        }
    }

    IEnumerator InstantiatePlayer(float seconds, Team team)
    {
        yield return new WaitForSeconds(seconds);
        Transform spawn = SpawnManager.instance.GetTeamSpawn(team);
        switch (team) //TODO: remove repeated team checking
        {
            case Team.Red:
                PhotonNetwork.Instantiate(RedPlayerPrefab.name, spawn.position, Quaternion.identity);
                break;
            case Team.Blue:
                PhotonNetwork.Instantiate(BluePlayerPrefab.name, spawn.position, Quaternion.identity);
                break;
        }
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.LogFormat("OnPlayerEnteredRoom() {0}", newPlayer.NickName);

        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient);
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.LogFormat("OnPlayerLeftRoom() {0}", otherPlayer.NickName); 

        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); 
        }
    }
}
