using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class KitbotMotor : RobotMotor
{
    [Header("Wheel Colliders")]
    public List<WheelCollider> totteringWheels;
    public List<WheelCollider> steeringWheels;

    [Header("Robot Settings")]
    public float motorForce = 300f;
    public float maxSteerAngle = 80f;

    public override void Steer(float horizontalInput)
    {
        foreach (WheelCollider wheel in steeringWheels)
        {
            wheel.steerAngle = horizontalInput * maxSteerAngle;
        }
    }

    public override void Accelerate(float virticalInput)
    {
        foreach(WheelCollider wheel in totteringWheels)
        {
            wheel.motorTorque = virticalInput * motorForce;
        }
    }
}
