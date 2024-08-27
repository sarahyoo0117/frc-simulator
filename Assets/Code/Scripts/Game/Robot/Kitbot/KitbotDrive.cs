using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitbotDrive : RobotDrive
{
    [Header("Wheel Colliders")]
    public List<WheelCollider> totteringWheels;
    public List<WheelCollider> steeringWheels;

    [Header("Robot Settings")]
    public float motorForce = 300f;
    public float maxSteerAngle = 80f;

    public override void Accelerate(Vector2 leftStick, Vector2 rightStick)
    {
        foreach(WheelCollider wheel in totteringWheels)
        {
            wheel.motorTorque = leftStick.y * motorForce;
        }
    }

    public override void Steer(Vector2 leftStick, Vector2 rightStick)
    {
        foreach (WheelCollider wheel in steeringWheels)
        {
            wheel.steerAngle = leftStick.x * maxSteerAngle;
        }
    }
}
