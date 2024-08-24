using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitbotController : RobotController
{
    [Header("Motor Settings")]
    public GameObject Motor1;
    public GameObject Motor2;

    private float z = 0f;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Update()
    {
        base.Update();
    }

    public override void Feed()
    {
        z -= Time.deltaTime * shootForce;
        RotateMotors();

        base.Feed();
    }

    public override void Shoot()
    {
        z += Time.deltaTime * shootForce;
        RotateMotors();

        base.Shoot();
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
