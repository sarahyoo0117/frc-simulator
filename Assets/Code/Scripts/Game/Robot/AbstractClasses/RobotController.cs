using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public abstract class RobotController : MonoBehaviour
{
    [Header("Loaded Note")]
    [SerializeField]
    public float shootForce = 300f;

    protected RobotNoteInsert m_insert;
    protected PhotonView robotPV;

    protected bool hasFeeded = false;
    protected bool isShootingNote = false;
    protected bool hasBeenShooted = false;

    protected virtual void Awake()
    {
        robotPV = GetComponent<PhotonView>();
        m_insert = GetComponent<RobotNoteInsert>();
    }

    protected virtual void Update()
    {
        if (isShootingNote)
        {
            ShootNote();
        }
        if (hasBeenShooted)
        {
            ResetAll();
        }
    }

    public virtual void Shoot()
    {
        if (hasFeeded)
            isShootingNote = true;
    }

    public virtual void Feed()
    {
        if (m_insert.loadedNote != null)
            hasFeeded = true;
    }

    public virtual void ShootNote()
    {
        robotPV.RPC("RPC_ShootNote", RpcTarget.AllBuffered);
    }


    public virtual void ResetAll()
    {
        robotPV.RPC("RPC_ResetAll", RpcTarget.AllBuffered);
    }

    [PunRPC]
    protected virtual void RPC_ShootNote()
    {
        Rigidbody noteRb = m_insert.loadedNote.GetComponent<Rigidbody>();
        Collider noteCollider = noteRb.GetComponent<Collider>();

        if (noteRb == null || noteCollider == null)
        {
            Debug.LogWarning("Note Rigidbody or Collider is missing!");
            return;
        }

        noteRb.isKinematic = false;
        noteCollider.enabled = true;
        noteRb.AddForce(m_insert.loadedNote.transform.forward * shootForce, ForceMode.Impulse);

        isShootingNote = false;
        hasBeenShooted = true;
    }

    [PunRPC]
    protected virtual void RPC_ResetAll()
    {
        m_insert.loadedNote.transform.parent = null;
        m_insert.loadedNote = null;
        m_insert.isLoaded = false;
        hasBeenShooted = false;
        hasFeeded = false;
    }
}
