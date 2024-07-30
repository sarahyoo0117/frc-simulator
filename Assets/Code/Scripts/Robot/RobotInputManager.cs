using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(RobotController))]
public class RobotInputManager : MonoBehaviour
{
    public RobotInput.OnFootActions onFoot;
    public bool isTeleop;

    private RobotController m_controller;
    private RobotInput m_input;

    private void Awake()
    {
        m_controller = GetComponent<RobotController>();

        m_input = new RobotInput();
        onFoot = m_input.OnFoot;

        if (isTeleop)
        {
            onFoot.Shoot.performed += ctx => m_controller.Shoot();
            onFoot.Feed.performed += ctx => m_controller.Feed();
        }
    }

    private void FixedUpdate()
    {
        if (isTeleop)
        {
            m_controller.Steer(onFoot.Movement.ReadValue<Vector2>().x);
            m_controller.Accelerate(onFoot.Movement.ReadValue<Vector2>().y);
        }
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
