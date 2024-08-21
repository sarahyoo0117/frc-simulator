using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.XR;

public class RobotNoteInsert : Interactable
{
    [Header("Note Settings")]
    public GameObject loadedNote;
    public Transform notePosition;
    public bool isLoaded;

    private PhotonView robotPV;
    private Animator m_animator;

    private void Awake()
    {
        robotPV = GetComponent<PhotonView>();
        m_animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (isLoaded && loadedNote != null)
        {
            loadedNote.transform.localPosition = Vector3.zero;
            loadedNote.transform.localRotation = Quaternion.identity;
        }
    }

    public void InsertNote(GameObject note , PlayerHand hand)
    {
        m_animator.Play("Kitbot_Shoot"); //adjust intial note position

        if (isLoaded == false && loadedNote == null)
        {
            PhotonView notePV = note.GetComponent<PhotonView>();
            PhotonView playerPV = hand.gameObject.GetComponent<PhotonView>();

            if (notePV != null)
            {
                robotPV.RPC("RPC_InsertNote", RpcTarget.AllBuffered, notePV.ViewID, playerPV.ViewID);
            }
        }
    }

    public void UnloadNote(PlayerHand hand)
    {
       if (isLoaded && loadedNote != null)
        {
            hand.PickupItem(loadedNote);
            robotPV.RPC("RPC_UnloadNote", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    private void RPC_InsertNote(int noteViewID, int playerViewID)
    {
        PhotonView notePV = PhotonView.Find(noteViewID);
        PhotonView playerPV = PhotonView.Find(playerViewID);

        if (notePV != null && playerPV != null)
        {
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
    private void RPC_UnloadNote()
    {
        loadedNote = null;
        isLoaded = false;
    }
}
