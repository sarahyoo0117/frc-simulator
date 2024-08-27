using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwerveDrive : RobotDrive
{
    public Wheel frontLeft, frontRight, backLeft, backRight;
    public float motorForce = 100f;

    public override void Rotate(float rightHorizontal)
    {
        base.Rotate(rightHorizontal);
    }

    public override void Steer(Vector2 leftStick, Vector2 rightStick)
    {
        float rightHorizontal = rightStick.x;

        frontLeft.targetAngle = GetVectorRotation(GetFrontLeftVector(leftStick, rightHorizontal));
        frontRight.targetAngle = GetVectorRotation(GetFrontRightVector(leftStick, rightHorizontal));
        backLeft.targetAngle = GetVectorRotation(GetBackLeftVector(leftStick, rightHorizontal));
        backRight.targetAngle = GetVectorRotation(GetBackRightVector(leftStick, rightHorizontal));
    }

    public override void Accelerate(Vector2 leftStick, Vector2 rightStick)
    {
        float rightHorizontal = rightStick.x;

        frontLeft.SetTorque(GetFrontLeftVector(leftStick, rightHorizontal).magnitude * motorForce);
        frontRight.SetTorque(GetFrontRightVector(leftStick, rightHorizontal).magnitude * motorForce);
        backLeft.SetTorque(GetBackLeftVector(leftStick, rightHorizontal).magnitude * motorForce);
        backRight.SetTorque(GetBackRightVector(leftStick, rightHorizontal).magnitude * motorForce);
    }

    private Vector2 ApplyRobotRotation(Vector2 originalVector)
    {
        float robotRotation = transform.eulerAngles.y;
        float angle = GetVectorRotation(originalVector);
        float magnitude = originalVector.magnitude;

        angle = SimplifyRotation(angle - robotRotation);

        return new Vector2(magnitude * Mathf.Cos(ToRadians(90 - angle)), magnitude * Mathf.Sin(ToRadians(90 - angle)));
    }

    private float GetVectorRotation(Vector2 vector)
    {
        return 90 - ToDegrees(Mathf.Atan2(vector.y, vector.x));
    }

    private float SimplifyRotation(float rotation)
    {
        while (rotation > 360.0f)
            rotation -= 360.0f;
        while (rotation < 0.0f)
            rotation += 360.0f;
        
        return rotation;
    }

    private float ToDegrees(float radians)
    {
        return (radians / Mathf.PI) * 180.0f;
    }

    private float ToRadians(float degrees)
    {
        return (degrees / 180.0f) * Mathf.PI;
    }

    private Vector2 GetFrontLeftVector(Vector2 leftStick, float rightHorizontal)
    {
        Vector2 turnVector = new Vector2(1.0f, 1.0f);
        turnVector.Normalize();
        turnVector *= rightHorizontal;

        return turnVector + ApplyRobotRotation(leftStick);
    }

    private Vector2 GetFrontRightVector(Vector2 leftStick, float rightHorizontal)
    {
        Vector2 turnVector = new Vector2(1.0f, -1.0f);
        turnVector.Normalize();
        turnVector *= rightHorizontal;

        return turnVector + ApplyRobotRotation(leftStick);
    }

    private Vector2 GetBackLeftVector(Vector2 leftStick, float rightHorizontal)
    {
        Vector2 turnVector = new Vector2(-1.0f, 1.0f);
        turnVector.Normalize();
        turnVector *= rightHorizontal;

        return turnVector + ApplyRobotRotation(leftStick);
    }

    private Vector2 GetBackRightVector(Vector2 leftStick, float rightHorizontal)
    {
        Vector2 turnVector = new Vector2(-1.0f, -1.0f);
        turnVector.Normalize();
        turnVector *= rightHorizontal;

        return turnVector + ApplyRobotRotation(leftStick);
    }
}
