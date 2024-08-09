using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class RobotMotor : MonoBehaviour
{
    [Header("Wheel Colliders")]
    public List<WheelCollider> totteringWheels;
    public List<WheelCollider> steeringWheels;

    [Header("Robot Settings")]
    public float motorForce = 300f;
    public float maxSteerAngle = 80f;
    public bool canSwerve;

    public void Steer(float horizontalInput)
    {
        foreach (WheelCollider wheel in steeringWheels)
        {
            wheel.steerAngle = horizontalInput * maxSteerAngle;
        }
    }

    public void Accelerate(float virticalInput)
    {
        foreach(WheelCollider wheel in totteringWheels)
        {
            wheel.motorTorque = virticalInput * motorForce;
        }
    }
}
