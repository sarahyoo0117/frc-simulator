using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerInteract : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private float distance = 3f;
    [SerializeField]
    private LayerMask mask;

    private Camera cam;
    private PlayerUI playerUI;
    private PlayerInputManager inputManager;
    private PlayerHand hand;

    void Start()
    {
        cam = GetComponent<PlayerLook>().cam;
        playerUI = GetComponent<PlayerUI>();
        inputManager = GetComponent<PlayerInputManager>();
        hand = GetComponent<PlayerHand>();
    }

    void Update()
    {
        playerUI.UpdateText(string.Empty);

        if (photonView.IsMine)
        {
            Ray ray = new Ray(cam.transform.position, cam.transform.forward);

            #if UNITY_EDITOR
            Debug.DrawRay(ray.origin, ray.direction * distance, Color.green);
            #endif

            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo, distance, mask))
            {
                Collider detectedObject = hitInfo.collider;
             
                if (detectedObject.TryGetComponent<Interactable>(out var interactable))
                {
                    HandleInteractable(interactable);
                }
                
                if (detectedObject.TryGetComponent<RobotNoteInsert>(out var robot))
                {
                    HandleRobotNoteInteraction(robot);
                }
            }
        }
    }

    private void HandleInteractable(Interactable interactable)
    {
        playerUI.UpdateText(interactable.promptMessage);

        if (inputManager.onFoot.Interact.triggered)
        {
            if (interactable is Pickable pickable)
            {
                hand.PickupItem(pickable.gameObject);
            }
            else if (interactable is DriverTrainTrigger driverTrig)
            {
                ActivateDriverTrain(driverTrig);
            }
            else
            {
                interactable.BaseInteract();
            }
        }
    }

    private void ActivateDriverTrain(DriverTrainTrigger driverTrig)
    {
        int playerViewID = photonView.ViewID;
        int robotViewID = (int)PhotonNetwork.LocalPlayer.CustomProperties["RobotViewID"];
        driverTrig.ActivateStation(playerViewID, robotViewID);
    }

    private void HandleRobotNoteInteraction(RobotNoteInsert robot)
    {
        if (hand.hasNote && robot.loadedNote == null)
        {
            playerUI.UpdateText("[E] to load Note");

            if (inputManager.onFoot.Interact.triggered)
            {
                robot.InsertNote(hand.Item, hand);
            }
        }
        else if (!hand.hasItem && robot.loadedNote != null)
        {
            playerUI.UpdateText("[E] to unload Note");

            if (inputManager.onFoot.Interact.triggered)
            {
                robot.UnloadNote(hand);
            }
        }
    }
}