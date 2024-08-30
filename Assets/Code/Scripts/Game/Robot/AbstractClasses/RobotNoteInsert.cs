using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public abstract class RobotNoteInsert : Interactable
{
    [Header("Note Settings")]
    public GameObject loadedNote;
    public Transform notePosition;
    public bool isLoaded;

    protected PhotonView robotPV;
    protected Animator m_animator;

    protected virtual void Awake()
    {
        robotPV = GetComponent<PhotonView>();
        m_animator = GetComponent<Animator>();
    }

    public virtual void InsertNote(GameObject note, PlayerHand hand)
    {
        if (!isLoaded && loadedNote == null)
        {
            PhotonView notePV = note.GetComponent<PhotonView>();
            PhotonView playerPV = hand.gameObject.GetComponent<PhotonView>();

            if (notePV != null)
            {
                robotPV.RPC("RPC_InsertNote", RpcTarget.AllBuffered, notePV.ViewID, playerPV.ViewID);
            }
        }
    }

    public virtual void UnloadNote(PlayerHand hand)
    {
        if (isLoaded && loadedNote != null)
        {
            hand.PickupItem(loadedNote);
            robotPV.RPC("RPC_UnloadNote", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    protected virtual void RPC_InsertNote(int noteViewID, int playerViewID)
    {
        PhotonView notePV = PhotonView.Find(noteViewID);
        PhotonView playerPV = PhotonView.Find(playerViewID);

        if (notePV != null && playerPV != null)
        {
            notePV.GetComponent<Rigidbody>().isKinematic = true;
            notePV.GetComponent<Collider>().enabled = false;

            Transform trans = notePV.transform;
            trans.parent = notePosition;
            trans.localPosition = Vector3.zero;
            trans.localRotation = Quaternion.identity;

            loadedNote = notePV.gameObject;
            isLoaded = true;

            PlayerHand hand = playerPV.GetComponent<PlayerHand>();
            hand.Item = null;
            hand.hasItem = false;
            hand.hasNote = false;
        }
    }

    [PunRPC]
    protected virtual void RPC_UnloadNote()
    {
        loadedNote = null;
        isLoaded = false;
    }
}

