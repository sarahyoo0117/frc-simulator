using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Trigger : Interactable
{
    [Header("Cameras")]
    public Camera Cam1 = null; //default player main cam
    public Camera Cam2; //drive station cam
    public Camera Cam3; //robot first person cam //TODO: how to assign a robot?
    public int CamManager;

    [Header("Play Settings")]
    public CharacterController playerController = null;
    public GameObject robot;

    private RobotInputManager robotInputManager;
    private Material triggerMaterial;
    private bool isPlaying;

    void Start()
    {
       triggerMaterial = GetComponent<Renderer>().material;
       robotInputManager = robot.GetComponent<RobotInputManager>();
       robotInputManager.isTeleop = false;
       playerController.enabled = true;
       ActivateCam1();
    }

    private void Update()
    {
        if (isPlaying)
        {
            //TODO: add GamePad Buttons
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                isPlaying = false;
                robotInputManager.isTeleop = false;
                triggerMaterial.color = Color.green;
                playerController.enabled = true;
                ActivateCam1();

                Cam1 = null;
                playerController = null;
            }
            else if (Input.GetKeyDown(KeyCode.F5))
            {
                ManageCamera();
            }
        }
    }

    protected override void Interact()
    {
        //Cam1 = player.GetComponent<PlayerLook>().cam;
        //playerController = player.GetComponent<CharacterController>();
        isPlaying = true;
        robotInputManager.isTeleop = true;
        triggerMaterial.color = Color.red;
        playerController.enabled = false;
        ActivateCam2();

    }

    public void ManageCamera()
    {
        if (CamManager == 1)
        {
            ActivateCam3();
        }
        else if (CamManager == 2)
        {
            ActivateCam2();
        }
    }

    void ActivateCam1()
    {
        Cam1.enabled = true;
        Cam2.enabled = false;      
        Cam3.enabled = false;
        CamManager = 0;
    }

    void ActivateCam2()
    {
        Cam1.enabled = false;
        Cam2.enabled = true;
        Cam3.enabled = false;
        CamManager = 1;
    }

    void ActivateCam3()
    {
        Cam1.enabled = false;
        Cam2.enabled = false;
        Cam3.enabled = true;
        CamManager = 2;
    }
}
