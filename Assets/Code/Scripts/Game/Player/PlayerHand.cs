using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerHand : MonoBehaviourPunCallbacks
{
    public GameObject hand;
    public GameObject Item;
    public bool hasItem;
    public bool hasNote;

    private PlayerInputManager m_inputManager;
    private GameObject worldObjectsHolder;

    private void Start()
    {
        m_inputManager = GetComponent<PlayerInputManager>();
        worldObjectsHolder = GameObject.FindGameObjectWithTag("WorldObjects");
    }

    private void Update()
    {
        if (hasItem)
            Item.transform.position = hand.transform.position;

        if (m_inputManager.onFoot.Dump.triggered)
        {
            DropItem();
        }
    }

    public void PickupItem(GameObject objectToPickUp)
    {
        PhotonView objectPV = objectToPickUp.GetComponent<PhotonView>();

        if (objectPV != null && hasItem == false)
        {
            photonView.RPC("RPC_PickUpItem", RpcTarget.AllBuffered, objectPV.ViewID);
        }
    }

    public void DropItem()
    {
        if (hasItem)
        {
            PhotonView itemPV = Item.GetComponent<PhotonView>();

            if (itemPV != null)
            {
                photonView.RPC("RPC_DropItem", RpcTarget.AllBuffered, itemPV.ViewID);
            }
        }
    }

    [PunRPC]
    private void RPC_PickUpItem(int objectViewID)
    {
        PhotonView objectPV = PhotonView.Find(objectViewID);

        if (objectPV != null)
        {
            Transform objectTrans= objectPV.transform;

            objectTrans.parent = hand.transform;
            objectTrans.position = Vector3.zero;
            objectTrans.rotation = Quaternion.identity;

            Rigidbody rb = objectPV.GetComponent<Rigidbody>();
            Collider col = objectPV.GetComponent<Collider>();

            if (rb != null && col != null)
            {
                rb.isKinematic = true;
                col.enabled = false;
            }

            Item = objectPV.gameObject;
            hasItem = true;

            if (Item.GetComponent<Note>() != null)
                hasNote = true;
        }
    }

    [PunRPC]
    private void RPC_DropItem(int objectViewID)
    {
        PhotonView objectPV = PhotonView.Find(objectViewID);

        Transform trans = objectPV.transform;
        trans.parent = worldObjectsHolder.transform;

        objectPV.GetComponent<Rigidbody>().isKinematic = false;
        objectPV.GetComponent<Collider>().enabled = true;

        Item = null;
        hasItem = false;
        if (hasNote) hasNote = false;
    }
}
