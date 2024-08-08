using Photon.Pun.Demo.Cockpit;
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
        if (photonView.IsMine)
        {
            playerUI.UpdateText(string.Empty);

            Ray ray = new Ray(cam.transform.position, cam.transform.forward);

            Debug.DrawRay(ray.origin, ray.direction * distance);

            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo, distance, mask)) 
            {
                Interactable interactable = hitInfo.collider.GetComponent<Interactable>();
                RobotNoteInsert insertable = hitInfo.rigidbody.GetComponent<RobotNoteInsert>();

                if (interactable != null && insertable == null)
                {
                    playerUI.UpdateText(interactable.promptMessage);

                    if (inputManager.onFoot.Interact.triggered)
                    {
                        Pickable pickable = hitInfo.collider.GetComponent<Pickable>();

                        if (pickable != null)
                        {
                            hand.PickupItem(pickable.gameObject);
                        }
                        else
                        {
                            interactable.BaseInteract();
                        }
                    }
                } 
                else if (insertable != null)
                {
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