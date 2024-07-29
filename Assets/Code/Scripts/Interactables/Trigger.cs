using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : Interactable
{
    public GameObject Cam1; //default player main cam
    public GameObject Cam2; //drive station cam
    public GameObject Cam3; //robot first person cam
    public int CamManager;

    public CharacterController playerController;
    public CharacterController robotController;

    Material triggerMaterial;
    private bool isPlaying;

    void Start()
    {
       triggerMaterial = gameObject.GetComponent<Renderer>().material;
       robotController.enabled = false;
       playerController.enabled = true;
    }

    private void Update()
    {
        if (isPlaying)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                isPlaying = false;
                triggerMaterial.color = Color.green;
                robotController.enabled = false;
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
        triggerMaterial.color = Color.red;
        playerController.enabled = false;
        robotController.enabled = true;
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
