using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager instance;

    //team spawn points
    GameObject[] redTeamSpawns;
    GameObject[] blueTeamSpawns;

    int r = 0;
    int b = 0;

    private void Start()
    {
        instance = this;
        redTeamSpawns = GameObject.FindGameObjectsWithTag("RedSpawn");
        blueTeamSpawns = GameObject.FindGameObjectsWithTag("BlueSpawn");
    }

    //TODO: assign spawn points randomly but not overlapped
    public Transform GetRedSpawn()
    {
        if (r >= redTeamSpawns.Length)
            r = 0;
        Transform spawn = redTeamSpawns[r].transform;
        r++;
        return spawn;
    }

    public Transform GetBlueSpawn()
    {
        if (b >= blueTeamSpawns.Length)
            b = 0;
        Transform spawn = blueTeamSpawns[b].transform;
        b++;
        return spawn;
    }

    public Transform GetTeamSpawn(Team team)
    {
        return team == Team.Red ? GetRedSpawn() : GetBlueSpawn();
    }
}
