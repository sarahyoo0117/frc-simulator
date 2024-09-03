using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitbotController : RobotController
{
    [Header("Motor Settings")]
    public GameObject Motor1;
    public GameObject Motor2;
    public float unFeededShootForce = 150f;

    [SerializeField]
    private bool hasFeeded;
    private float Z = 0f;

    protected override void Awake()
    {
        base.Awake();
    }

    public override void Feed()
    {
        Z -= Time.deltaTime * shootForce;
        RotateMotors();
        
        if (m_insert.isLoaded)
            hasFeeded = true;
    }

    public override void Shoot()
    {
        Z += Time.deltaTime * shootForce;
        RotateMotors();

        if (hasFeeded)
        {
            base.Shoot();
            hasFeeded = false;
        }
        else if (m_insert.isLoaded && !hasFeeded)
        {
            ShootNote(unFeededShootForce);
        }
    }

    private void RotateMotors()
    {
        if (Motor1 && Motor2)
        {
            Motor1.transform.localRotation = Quaternion.Euler(0, 0, Z);
            Motor2.transform.localRotation = Quaternion.Euler(0, 0, Z);
        }
    }
}
