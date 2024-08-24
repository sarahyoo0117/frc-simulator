using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public abstract class RobotController : MonoBehaviour
{
    public static RobotController instance;

    [Header("Loaded Note")]
    [SerializeField]
    public GameObject note;
    public float shootForce = 300f;

    protected RobotNoteInsert m_insert;
    protected PhotonView robotPV;

    protected bool hasFeeded = false;
    protected bool isShootingNote = false;
    protected bool hasBeenShooted = false;

    protected virtual void Awake()
    {
        instance = this;
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
        if (note != null)
            hasFeeded = true;
    }

    public virtual void Feed()
    {
        if (hasFeeded)
            isShootingNote = true;
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
        Rigidbody noteRb = note.GetComponent<Rigidbody>();
        Collider noteCollider = noteRb.GetComponent<Collider>();

        if (noteRb == null || noteCollider == null)
        {
            Debug.LogWarning("Note Rigidbody or Collider is missing!");
            return;
        }

        noteRb.isKinematic = false;
        noteCollider.enabled = true;
        noteRb.AddForce(note.transform.forward * shootForce, ForceMode.Impulse);

        isShootingNote = false;
        hasBeenShooted = true;
    }

    [PunRPC]
    protected virtual void RPC_ResetAll()
    {
        m_insert.isLoaded = false;
        m_insert.loadedNote = null;
        note.transform.parent = null;
        note = null;
        hasBeenShooted = false;
        hasFeeded = false;
    }
}
