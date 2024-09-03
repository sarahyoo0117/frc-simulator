using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public abstract class RobotController : MonoBehaviour
{
    [Header("Shoot Settings")]
    [SerializeField]
    public float shootForce = 300f;
    public bool readyToShoot;

    protected RobotNoteInsert m_insert;
    protected PhotonView robotPV;
    protected GameObject worldObjectsHolder;

    protected virtual void Awake()
    {
        m_insert = GetComponent<RobotNoteInsert>();
        robotPV = GetComponent<PhotonView>();
        worldObjectsHolder = GameObject.FindGameObjectWithTag("WorldObjects");
    }

    public virtual void Feed()
    {

    }

    public virtual void Shoot()
    {
        if (m_insert.isLoaded)
            ShootNote(shootForce);
    }

    public virtual void ShootNote(float shootForce)
    {
        robotPV.RPC("RPC_ShootNote", RpcTarget.AllBuffered, shootForce);
    }

    public virtual void ResetAll()
    {
        robotPV.RPC("RPC_ResetAll", RpcTarget.AllBuffered);
    }

    [PunRPC]
    protected virtual void RPC_ShootNote(float shootForce)
    {
        if (m_insert.loadedNote == null) return;

        Rigidbody noteRb = m_insert.loadedNote.GetComponent<Rigidbody>();
        Collider noteCollider = noteRb.GetComponent<Collider>();

        noteRb.isKinematic = false;
        noteCollider.enabled = true;
        noteRb.AddForce(m_insert.loadedNote.transform.forward * shootForce, ForceMode.Impulse);

        ResetAll();
    }

    [PunRPC]
    protected virtual void RPC_ResetAll()
    {
        m_insert.loadedNote.transform.parent = worldObjectsHolder.transform;
        m_insert.loadedNote = null;
        m_insert.isLoaded = false;
    }

    public virtual void OnShootAnimationComplete()
    {
        readyToShoot = true;
    }
}
