using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks
{
    public GameObject RedPlayerPrefab;
    public GameObject BluePlayerPrefab;
    public SpawnManager spawnManager;

    void Start()
    {
        spawnManager = FindAnyObjectByType<SpawnManager>();

        if (PlayerManager.LocalPlayerInstance == null)
        {
            Team team = (Team)PhotonNetwork.LocalPlayer.CustomProperties["Team"];
            string myRobotName = PlayerPrefs.GetString("SelectedRobot");

            StartCoroutine(InstantiatePlayerAndRobot(2, team, myRobotName));        
        }
    }

    IEnumerator InstantiatePlayerAndRobot(float seconds, Team team, string robotPrefabName)
    {
        yield return new WaitForSeconds(seconds);
    
        //Spawn player first to update spanw indices
        Transform playerSpawn = spawnManager.GetPlayerSpawn(team);
        Transform robotSpawn = spawnManager.GetRobotSpawn(team);
       
        GameObject playerPrefab = (team == Team.Red) ? RedPlayerPrefab : BluePlayerPrefab;
        
        PhotonNetwork.Instantiate(playerPrefab.name, playerSpawn.position, playerSpawn.rotation);
        GameObject robotInstance = PhotonNetwork.Instantiate(robotPrefabName, robotSpawn.position, robotSpawn.rotation);

        int robotViewID = robotInstance.GetComponent<PhotonView>().ViewID;
        ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable();
        props.Add("RobotViewID", robotViewID);
        PhotonNetwork.SetPlayerCustomProperties(props);
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }
}
