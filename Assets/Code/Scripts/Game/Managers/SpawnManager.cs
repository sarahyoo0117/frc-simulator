using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager instance;

    [Header("Player Spawnpoints")]
    public Transform[] playerRedSpawns;
    public Transform[] playerBlueSpawns;

    [Header("Robot Spawnpoints")]
    public Transform[] robotRedSpawns;
    public Transform[] robotBlueSpawns;

    //player indeces
    int rP = 0;
    int bP = 0;
    //robot indeces
    int rR, bR;

    private void Awake()
    {
        instance = this;
        rR = rP;
        bR = bP;
    }

    public Transform GetPlayerRedSpawn()
    {
        if (rP >= playerRedSpawns.Length)
            rP = 0;
        rR = rP;
        Transform spawn = playerRedSpawns[rP];
        rP++;
        return spawn;
    }

    public Transform GetPlayerBlueSpawn()
    {
        if (bP >= playerBlueSpawns.Length)
            bP = 0;
        bR = bP;
        Transform spawn = playerBlueSpawns[bP];
        bP++;
        return spawn;
    }

    //*** always spawn player first to update robot indeces.
    public Transform GetRobotRedSpawn()
    {
        Transform spawn = robotRedSpawns[rR];
        return spawn;
    }

    public Transform GetRobotBlueSpawn()
    {
        Transform spawn = robotBlueSpawns[bR];
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