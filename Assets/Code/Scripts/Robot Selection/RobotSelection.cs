using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RobotSelection : MonoBehaviour
{
    public GameObject[] Robots;
    public int currentRobot = 0;

    private void Start()
    {
        Robots[currentRobot].SetActive(true);
    }

    public void NextRobot()
    {
        Robots[currentRobot].SetActive(false);
        currentRobot = (currentRobot + 1) % Robots.Length;
        Robots[currentRobot].SetActive(true);
    }

    public void PreviousRobot()
    {
        Robots[currentRobot].SetActive(false);
        currentRobot = (currentRobot - 1 + Robots.Length) % Robots.Length;
        Robots[currentRobot].SetActive(true);
    }

    public void SelectRobot() //TODO
    {
        string robotPrefabName = Robots[currentRobot].name;
        Debug.Log($"Chose {robotPrefabName}.");
        PlayerPrefs.SetString("SelectedRobot", robotPrefabName);
    }

    public void ExitScene()
    {
        SceneManager.LoadScene("Lobby");
    }
}
