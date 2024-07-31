using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RobotInputManager : MonoBehaviour
{
    public RobotInput.OnFootActions onFoot;
    public bool isTeleop;

    private RobotMotor m_motor;
    private RobotInput m_input;
    private RobotController m_controller;

    public bool isFeeding;
    public bool isShooting;

    private void Awake()
    {
        m_input = new RobotInput();
        onFoot = m_input.OnFoot;

        m_motor = GetComponent<RobotMotor>();
        m_controller = GetComponent<RobotController>();
    }

    private void FixedUpdate()
    {
        if (isTeleop)
        {
            m_motor.Steer(onFoot.Movement.ReadValue<Vector2>().x);
            m_motor.Accelerate(onFoot.Movement.ReadValue<Vector2>().y);

            if (isFeeding && !isShooting)
            {
                m_controller.Feed();
            }

            if (isShooting && !isFeeding)
            {
                m_controller.Shoot();
            }
        }
    }

    private void OnEnable()
    {
        onFoot.Enable();

        onFoot.Feed.started += ctx => isFeeding = true;
        onFoot.Feed.canceled += ctx => isFeeding = false;
        onFoot.Shoot.started += ctx => isShooting = true;
        onFoot.Shoot.canceled += ctx => isShooting = false;
    }

    private void OnDisable()
    {
        onFoot.Disable();
    }
}
