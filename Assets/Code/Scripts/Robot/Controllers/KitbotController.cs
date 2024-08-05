using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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
    private Collider m_collider;

    private float z = 0f;
    private bool hasFeeded = false;
    private bool isShootingNote = false;
    private bool hasBeenShooted = false;

    private void Start()
    {
        m_insert = GetComponent<RobotNoteInsert>();
        m_collider = GetComponent<Collider>();
    }

    private void Update()
    {
        if (m_insert.loadedNote != null && note == null)
        {
            note = m_insert.loadedNote;
            Physics.IgnoreCollision(m_collider, note.GetComponent<Collider>()); //TODO
        }

        if (isShootingNote)
        {
            ShootNote();
        }

        if (hasBeenShooted)
        {
            m_insert.isLoaded = false;
            m_insert.loadedNote = null;
            note.transform.parent = null;
            note = null;
            hasBeenShooted = false;
            hasFeeded = false;
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
        Rigidbody noteRb = note.GetComponent<Rigidbody>();
        Collider noteCollider = noteRb.GetComponent<Collider>();

        if (noteRb != null && noteCollider != null)
        {
            noteRb.isKinematic = false;
            noteCollider.enabled = true;
            noteRb.AddForce(note.transform.forward * rotateSpeed, ForceMode.Impulse);
        }
        else
        {
            print("Does not have rb");
        }

        isShootingNote = false;
        hasBeenShooted = true;
    }

    private void RotateMotors()
    {
        if (Motor1 && Motor2)
        {
            Motor1.transform.localRotation = Quaternion.Euler(90, 0, z);
            Motor2.transform.localRotation = Quaternion.Euler(90, 0, z);
        }
    }
}
