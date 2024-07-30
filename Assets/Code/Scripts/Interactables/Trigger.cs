using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : Interactable
{
    [Header("Cameras")]
    public GameObject Cam1; //default player main cam
    public GameObject Cam2; //drive station cam
    public GameObject Cam3; //robot first person cam
    public int CamManager;

    [Header("Play Settings")]
    public CharacterController playerController;
    public GameObject robot;

    private Material triggerMaterial;
    private bool isPlaying;
    private RobotInputManager robotInputManager;

    void Start()
    {
       triggerMaterial = gameObject.GetComponent<Renderer>().material;
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
            }
            else if (Input.GetKeyDown(KeyCode.F5))
            {
                ManageCamera();
            }
        }
    }

    protected override void Interact()
    {
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
        Cam1.SetActive(true);
        Cam2.SetActive(false);
        Cam3.SetActive(false);
        CamManager = 0;
    }

    void ActivateCam2()
    {
        Cam1.SetActive(false);
        Cam2.SetActive(true);
        Cam3.SetActive(false);
        CamManager = 1;
    }

    void ActivateCam3()
    {
        Cam1.SetActive(false);
        Cam2.SetActive(false);
        Cam3.SetActive(true);
        CamManager = 2;
    }
}
