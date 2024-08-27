using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RobotDrive : MonoBehaviour
{
    public float rotateSpeed = 300f;

    public virtual void Steer(Vector2 leftStick, Vector2 rightStick)
    {

    }

    public virtual void Rotate(float rightHorizontal)
    {
        transform.Rotate(Vector3.up * rightHorizontal * rotateSpeed * Time.deltaTime);
    }

    public virtual void Accelerate(Vector2 leftStick, Vector2 rightStick)
    {

    }

}
