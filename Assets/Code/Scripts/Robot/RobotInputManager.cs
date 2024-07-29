using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RobotInputManager : MonoBehaviour
{
    private RobotInput robotInput;
    public RobotInput.OnFootActions onFoot;

    private RobotMotor motor;

    private void Awake()
    {
        robotInput = new RobotInput();
        onFoot = robotInput.OnFoot;

        motor = GetComponent<RobotMotor>();

        onFoot.Shoot.performed += ctx => motor.Shoot();
        onFoot.Feed.performed += ctx => motor.Feed();
    }

    private void FixedUpdate()
    {
        motor.ProcessMove(onFoot.Movement.ReadValue<Vector2>());
    }

    private void LateUpdate()
    {
        motor.ProcessRotation(onFoot.Look.ReadValue<Vector2>());
    }

    private void OnEnable()
    {
        onFoot.Enable();
    }

    private void OnDisable()
    {
        onFoot.Disable();
    }
}
