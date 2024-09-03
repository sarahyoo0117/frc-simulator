using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnManager : MonoBehaviour
{
    [Header("Player Spawnpoints")]
    public Transform[] playerRedSpawns;
    public Transform[] playerBlueSpawns;

    [Header("Robot Spawnpoints")]
    public Transform[] robotRedSpawns;
    public Transform[] robotBlueSpawns;

    ExitGames.Client.Photon.Hashtable roomProperties;
    int redSpawnIndex = 0;
    int blueSpawnIndex = 0;

    private void Awake()
    {
        roomProperties = new ExitGames.Client.Photon.Hashtable()
        {
            {"RedSpawnIndex", redSpawnIndex },
            {"BlueSpawnIndex", blueSpawnIndex }
        };
        PhotonNetwork.CurrentRoom.SetCustomProperties(roomProperties);
    }

    //Spawn player first.
    private Transform GetPlayerRedSpawn()
    {
        redSpawnIndex = (int)PhotonNetwork.CurrentRoom.CustomProperties["RedSpawnIndex"];

        if (redSpawnIndex >= playerRedSpawns.Length)
            redSpawnIndex = 0;

        Transform spawn = playerRedSpawns[redSpawnIndex];
        return spawn;
    }

    private Transform GetPlayerBlueSpawn()
    {
        blueSpawnIndex = (int)PhotonNetwork.CurrentRoom.CustomProperties["RedSpawnIndex"];

        if (blueSpawnIndex >= playerBlueSpawns.Length)
            blueSpawnIndex = 0;

        Transform spawn = playerBlueSpawns[blueSpawnIndex];
        return spawn;
    }

    private Transform GetRobotRedSpawn()
    {
        Transform spawn = robotRedSpawns[redSpawnIndex];
        redSpawnIndex++;
        roomProperties["RedSpawnIndex"] = redSpawnIndex;
        PhotonNetwork.CurrentRoom.SetCustomProperties(roomProperties);
        return spawn;
    }

    private Transform GetRobotBlueSpawn()
    {
        Transform spawn = robotBlueSpawns[blueSpawnIndex];
        blueSpawnIndex++;
        roomProperties["BlueSpawnIndex"] = blueSpawnIndex;
        PhotonNetwork.CurrentRoom.SetCustomProperties(roomProperties);
        return spawn;
    }

    public Transform GetPlayerSpawn(Team team)
    {
        return team == Team.Red ? GetPlayerRedSpawn() : GetPlayerBlueSpawn();
    }

    public Transform GetRobotSpawn(Team team)
    {
        return team == Team.Red ? GetRobotRedSpawn() : GetRobotBlueSpawn();
    }
}