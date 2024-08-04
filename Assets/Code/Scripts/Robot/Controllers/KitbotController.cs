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

    private float z = 0f;

    public override void Feed()
    {
        z -= Time.deltaTime * rotateSpeed;
        RotateMotors();
    }

    public override void Shoot()
    {
        z += Time.deltaTime * rotateSpeed;
        RotateMotors();
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
