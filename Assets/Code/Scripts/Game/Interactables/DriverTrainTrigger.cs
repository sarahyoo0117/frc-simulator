using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DriverTrainTrigger : Interactable
{
    [Header("Cameras")]
    public Camera Cam1 = null; //default player main cam
    public Camera Cam2; //drive station cam
    public Camera Cam3; //robot first person cam //TODO: how to assign a robot?
    public int CamManager;

    [Header("Play Settings")] //TODO
    public CharacterController playerController = null;
    public GameObject robot; 

    private RobotInputManager robotInputManager;
    private PhotonView m_PV;
    private Material triggerMaterial;
    private bool isPlaying;

    void Start()
    {
       triggerMaterial = GetComponent<Renderer>().material;
       robotInputManager = robot.GetComponent<RobotInputManager>();
       m_PV = GetComponent<PhotonView>();

       robotInputManager.isTeleop = false;
       ActivateCam1();
    }

    private void Update()
    {
        if (isPlaying)
        {
            //TODO: add GamePad Buttons
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ExitStation();
            }
            else if (Input.GetKeyDown(KeyCode.F5))
            {
                ManageCamera();
            }
        }
    }

    public void ActivateStation(PhotonView playerPV)
    {
        m_PV.RPC("RPC_ActivateStation", RpcTarget.AllBuffered, playerPV.ViewID);
    }

    [PunRPC]
    private void RPC_ActivateStation(int playerViewID)
    {
        PhotonView playerPV = PhotonView.Find(playerViewID);

        Cam1 = playerPV.GetComponent<PlayerLook>().cam;
        playerController = playerPV.GetComponent<CharacterController>();

        isPlaying = true;
        Cam1.enabled = false;
        playerController.enabled = false;
        robotInputManager.isTeleop = true;
        triggerMaterial.color = Color.red;
        ActivateCam2();
    }

    public void ExitStation()
    {
        m_PV.RPC("RPC_ExitStation", RpcTarget.AllBuffered);
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

    public void ActivateCam1()
    {
        m_PV.RPC("RPC_ActivateCam1", RpcTarget.AllBuffered);
    }

    public void ActivateCam2()
    {
        m_PV.RPC("RPC_ActivateCam2", RpcTarget.AllBuffered);
    }

    public void ActivateCam3()
    {
        m_PV.RPC("RPC_ActivateCam3", RpcTarget.AllBuffered);
    }

    [PunRPC]
    private void RPC_ExitStation()
    {
        isPlaying = false;
        robotInputManager.isTeleop = false;
        triggerMaterial.color = Color.green;
        playerController.enabled = true;
        ActivateCam1();

        Cam1 = null;
        playerController = null;
    }

    [PunRPC]
    private void RPC_ActivateCam1()
    {
        Cam1.enabled = true;
        Cam2.enabled = false;
        Cam3.enabled = false;
        CamManager = 0;
    }

    [PunRPC]
    private void RPC_ActivateCam2()
    {
        Cam1.enabled = false;
        Cam2.enabled = true;
        Cam3.enabled = false;
        CamManager = 1;
    }

    [PunRPC]
    private void RPC_ActivateCam3()
    {
        Cam1.enabled = false;
        Cam2.enabled = false;
        Cam3.enabled = true;
        CamManager = 2;
    }
}
