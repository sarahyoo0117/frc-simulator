using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class IntakeTrigger : MonoBehaviour
{
    [SerializeField]
    private RobotNoteInsert m_insert;
    [SerializeField]
    private PhotonView robotPV;

    private void Awake()
    {
        m_insert = GetComponent<RobotNoteInsert>();
        robotPV = GetComponent<PhotonView>();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Note"))
        {
            FeedNote(other);
        }
    }

    public void FeedNote(Collider note)
    {
        PhotonView notePV = note.GetComponent<PhotonView>();
        Debug.Log(notePV.ViewID);
        robotPV.RPC("RPC_FeedNote", RpcTarget.AllBuffered, notePV.ViewID);
    }

    [PunRPC]
    private void RPC_FeedNote(int noteViewID)
    {
        PhotonView notePV = PhotonView.Find(noteViewID);

        notePV.GetComponent<Rigidbody>().isKinematic = true;
        notePV.GetComponent<Collider>().enabled = false;

        Transform trans = notePV.transform;
        trans.parent = m_insert.notePosition;
        trans.localPosition = Vector3.zero;
        trans.localRotation = Quaternion.identity;

        m_insert.loadedNote = notePV.gameObject;
        m_insert.isLoaded = true;
    }
}
