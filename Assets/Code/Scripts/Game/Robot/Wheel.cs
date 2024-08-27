using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheel : MonoBehaviour
{
    [Header("Wheel Settings")]
    public WheelCollider col;
    public double rotationSpeed = 10.0f;
    public double targetAngle = 0.0f;

    public void SetTorque(double torque)
    {
        col.motorTorque = (float)torque;
    }

    private double SimplifyAngle(double degrees)
    {
        while (degrees > 360.0f)
            degrees -= 360.0f;
        while (degrees < 0.0f)
            degrees += 360.0f;
        return degrees;
    }

    private double ClosestAngleTransform(double current, double target)
    {
        if (target == current)
            return 0.0f;
        else if (target > current)
        {
            if (target - 180.0f > current)
                current += 360.0f;
            return target - current;
        }
        else
        {
            if (target + 180.0f < current)
                target += 360.0f;
            return target - current;
        }
    }

    private void Update()
    {
        double angleTransform = ClosestAngleTransform(col.steerAngle, targetAngle);
        if (Mathf.Abs((float)angleTransform) < rotationSpeed)
            col.steerAngle = (float)SimplifyAngle(col.steerAngle + angleTransform);
        else
            col.steerAngle = (float)SimplifyAngle(col.steerAngle + rotationSpeed * Mathf.Sign((float)angleTransform));
    }

    private void FixedUpdate()
    {
        Vector3 pos = transform.position;
        Quaternion quat = transform.rotation;

        col.GetWorldPose(out pos, out quat);

        transform.position = pos;
        transform.rotation = quat;
    }
}