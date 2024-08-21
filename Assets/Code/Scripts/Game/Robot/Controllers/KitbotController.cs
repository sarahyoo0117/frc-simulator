using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Photon.Pun;

public class KitbotController : RobotController
{
    [Header("Motor Settings")]
    public GameObject Motor1;
    public GameObject Motor2;
    public float rotateSpeed = 300f;

    [Header("Loaded Note")]
    [SerializeField]
    private GameObject note;

    private RobotNoteInsert m_insert;
    private PhotonView robotPV;

    private float z = 0f;
    private bool hasFeeded = false;
    private bool isShootingNote = false;
    private bool hasBeenShooted = false;

    private void Awake()
    {
        robotPV = GetComponent<PhotonView>();
        m_insert = GetComponent<RobotNoteInsert>();
    }

    private void Update()
    {
        IgnoreCollsionWithNote();

        if (isShootingNote)
        {
            ShootNote();
        }

        if (hasBeenShooted)
        {
            ResetAll();
        }
    }

    public override void Feed()
    {
        z -= Time.deltaTime * rotateSpeed;
        RotateMotors();

        if (note != null)
            hasFeeded = true;
    }

    public override void Shoot()
    {
        z += Time.deltaTime * rotateSpeed;
        RotateMotors();

        if (hasFeeded)
            isShootingNote = true;
    }

    private void ShootNote()
    {
        robotPV.RPC("RPC_ShootNote", RpcTarget.AllBuffered);
    }

    private void RotateMotors()
    {
        if (Motor1 && Motor2)
        {
            Motor1.transform.localRotation = Quaternion.Euler(90, 0, z);
            Motor2.transform.localRotation = Quaternion.Euler(90, 0, z);
        }
    }

    private void IgnoreCollsionWithNote()
    {
        if (m_insert.loadedNote != null && note == null)
        {
            note = m_insert.loadedNote;

            Collider noteCol = note.GetComponent<Collider>();
            Collider[] _colliders = GetComponentsInChildren<Collider>();

            foreach(Collider collider in _colliders)
            {
                Physics.IgnoreCollision(collider, noteCol);
            }
        }
    }

    public void ResetAll()
    {
        robotPV.RPC("RPC_ResetAll", RpcTarget.AllBuffered);
    }

    [PunRPC]
    private void RPC_ShootNote()
    {
        Rigidbody noteRb = note.GetComponent<Rigidbody>();
        Collider noteCollider = noteRb.GetComponent<Collider>();

        if (noteRb != null && noteCollider != null)
        {
            noteRb.isKinematic = false;
            noteCollider.enabled = true;
            noteRb.AddForce(note.transform.forward * rotateSpeed, ForceMode.Impulse);

            isShootingNote = false;
            hasBeenShooted = true;
        }
    }

    [PunRPC]
    private void RPC_ResetAll()
    {
        m_insert.isLoaded = false;
        m_insert.loadedNote = null;
        note.transform.parent = null;
        note = null;
        hasBeenShooted = false;
        hasFeeded = false;
    }

}
