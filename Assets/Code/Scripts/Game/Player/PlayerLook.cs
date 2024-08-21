using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerLook : MonoBehaviourPunCallbacks
{
    public Camera cam;
    public AudioListener audioListener;
    public GameObject hand;
    private float xRotation = 0f;

    public float xSensitivity = 30f;
    public float ySensitivity = 30f;

    private void Awake()
    {
        cam = GetComponentInChildren<Camera>();
    }

    private void Start()
    {
        if (!photonView.IsMine)
        {
            cam.enabled = false;
            //audioListener.enabled = false;
        }
    }

    public void ProcessLook(Vector2 input)
    {
        float mouseX = input.x;
        float mouseY = input.y;

        xRotation -= mouseY * ySensitivity;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        cam.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

        transform.Rotate(Vector3.up * mouseX * xSensitivity);
    }
}
