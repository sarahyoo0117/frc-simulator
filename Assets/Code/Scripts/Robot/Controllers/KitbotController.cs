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

    [Header("Note Settings")]
    public GameObject Note;
    public Transform ReadyNoteTransform;
    public Transform FeededNoteTransform;

    private float z = 0f;

    public override void Feed()
    {
        z -= Time.deltaTime * rotateSpeed;
        RotateMotors();

        if (Note != null)
        {

            Note.transform.position = FeededNoteTransform.position;
            Note.transform.rotation = FeededNoteTransform.rotation;
        }
    }

    public override void Shoot()
    {
        z += Time.deltaTime * rotateSpeed;
        RotateMotors();

        if (Note != null)
        {
            Note.transform.position = ReadyNoteTransform.position;
            Note.transform.rotation = ReadyNoteTransform.rotation;
        }
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
