using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class LevelManager : MonoBehaviourPunCallbacks
{
    public static LevelManager instance;

    [SerializeField]
    private GameObject loaderPanel;

    private void Awake()
    {
        instance = this;
    }

    public void LoadScene(string sceneName)
    {
        var scene = SceneManager.LoadSceneAsync(sceneName);
        loaderPanel.SetActive(true);

        scene.allowSceneActivation = true;
        loaderPanel.SetActive(false);
    }

    public void LoadPhotonLevel(string sceneName)
    {
        loaderPanel.SetActive(true);
        PhotonNetwork.LoadLevel(sceneName);
    }
}
