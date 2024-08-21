using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DriverTrainTrigger : Interactable
{
    [Header("Cameras")]
    public Camera Cam1; //player main cam
    public Camera Cam2; //drive station cam
    public Camera Cam3; //robot main cam
    public int CamManager;

    [Header("Play Settings")] 
    [SerializeField]
    private CharacterController playerController;
    [SerializeField]
    private RobotInputManager robotInputManager;

    private PhotonView m_PV;
    private Material triggerMaterial;
    private bool isPlaying;
    private PhotonView playerPV;

    private void Awake()
    {
       triggerMaterial = GetComponent<Renderer>().material;
       m_PV = GetComponent<PhotonView>();
       Cam2.enabled = false;
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

    public void ActivateStation(int playerViewID, int robotViewID)
    {
        m_PV.RPC("RPC_ActivateStation", RpcTarget.AllBuffered, playerViewID, robotViewID);
    }

    public void ExitStation()
    {
        m_PV.RPC("RPC_ExitStation", RpcTarget.AllBuffered);
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
    private void RPC_ActivateStation(int playerViewID, int robotViewID)
    {
        playerPV = PhotonView.Find(playerViewID);
        PhotonView robotPV = PhotonView.Find(robotViewID);

        if (playerPV.IsMine)
        {
            Cam1 = playerPV.GetComponentInChildren<Camera>();
            Cam3 = robotPV.GetComponentInChildren<Camera>();
            robotInputManager = robotPV.GetComponent<RobotInputManager>();
            playerController = playerPV.GetComponent<CharacterController>();
        
            isPlaying = true;
            playerController.enabled = false;
            robotInputManager.isTeleop = true;
            triggerMaterial.color = Color.red;
            ActivateCam2();
        }
    }

    [PunRPC]
    private void RPC_ExitStation()
    {
        if (playerPV.IsMine)
        {
            isPlaying = false;
            robotInputManager.isTeleop = false;
            triggerMaterial.color = Color.green;
            playerController.enabled = true;
            ActivateCam1();

            Cam1 = null;
            Cam3 = null;
            playerPV = null;
            robotInputManager = null;
            playerController = null;
        }
    }

    [PunRPC]
    private void RPC_ActivateCam1()
    {
        if (playerPV.IsMine)
        {
            Cam1.enabled = true;
            Cam2.enabled = false;
            Cam3.enabled = false;
            CamManager = 0;
        }
    }

    [PunRPC]
    private void RPC_ActivateCam2()
    {
        if (playerPV.IsMine)
        {
            Cam1.enabled = false;
            Cam2.enabled = true;
            Cam3.enabled = false;
            CamManager = 1;
        }
    }

    [PunRPC]
    private void RPC_ActivateCam3()
    {
        if (playerPV.IsMine)
        {
            Cam1.enabled = false;
            Cam2.enabled = false;
            Cam3.enabled = true;
            CamManager = 2;
        }
    }
}
