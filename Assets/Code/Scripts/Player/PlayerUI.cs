using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class PlayerUI : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private TextMeshProUGUI promptText;

    private void Start()
    {
        if (!photonView.IsMine)
        {
            promptText.enabled = false;
        }
    }
    public void UpdateText(string promptMessage)
    {
        promptText.text = promptMessage;
    }
}