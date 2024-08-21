using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using ExitGames.Client.Photon;

public class GameManager : MonoBehaviourPunCallbacks
{
    public GameObject RedPlayerPrefab;
    public GameObject BluePlayerPrefab;

    void Start()
    {
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
        //*** always get player spawn first.
        Transform playerSpawn = SpawnManager.instance.GetPlayerSpawn(team);
        Transform robotSpawn = SpawnManager.instance.GetRobotSpawn(team);
       
        GameObject playerPrefab = (team == Team.Red) ? RedPlayerPrefab : BluePlayerPrefab;
        
        PhotonNetwork.Instantiate(playerPrefab.name, playerSpawn.position, Quaternion.identity);
        GameObject robotInstance = PhotonNetwork.Instantiate(robotPrefabName, robotSpawn.position, Quaternion.identity);

        int robotViewID = robotInstance.GetComponent<PhotonView>().ViewID;
        ExitGames.Client.Photon.Hashtable prop = new ExitGames.Client.Photon.Hashtable();
        prop["RobotViewID"] = robotViewID;
        PhotonNetwork.SetPlayerCustomProperties(prop);
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
