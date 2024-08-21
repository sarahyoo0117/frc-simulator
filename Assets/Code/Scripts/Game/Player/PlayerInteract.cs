using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

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
                Interactable interactable = hitInfo.collider.GetComponent<Interactable>();

                if (interactable != null)
                {
                    playerUI.UpdateText(interactable.promptMessage);

                    if (inputManager.onFoot.Interact.triggered)
                    {
                        Pickable pickable = hitInfo.collider.GetComponent<Pickable>();
                        DriverTrainTrigger driverTrig = hitInfo.collider.GetComponent<DriverTrainTrigger>();
                        if (pickable != null)
                        {
                            hand.PickupItem(pickable.gameObject);
                        }
                        else if (driverTrig != null)
                        {
                            int playerViewID = photonView.ViewID;
                            int robotViewID = (int)PhotonNetwork.LocalPlayer.CustomProperties["RobotViewID"];
                            driverTrig.ActivateStation(playerViewID, robotViewID);
                        }
                        else
                        {
                            interactable.BaseInteract();
                        }
                    }
                    return;
                }

                if (hitInfo.rigidbody != null)
                {
                    RobotNoteInsert insertable = hitInfo.rigidbody.GetComponent<RobotNoteInsert>();

                    if (insertable == null) return;

                    if (hand.hasNote && insertable.loadedNote == null)
                    {
                        playerUI.UpdateText("[E] to load Note");

                        if (inputManager.onFoot.Interact.triggered)
                        {
                            insertable.InsertNote(hand.Item, hand);
                        }
                    }
                    else if (!hand.hasItem && insertable.loadedNote != null)
                    {
                        playerUI.UpdateText("[E] to unload Note");

                        if (inputManager.onFoot.Interact.triggered)
                        {
                            insertable.UnloadNote(hand);
                        }
                    }
                }
            }
        }
    }
}