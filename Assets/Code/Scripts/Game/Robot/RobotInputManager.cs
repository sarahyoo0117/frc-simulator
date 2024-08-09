using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RobotInputManager : MonoBehaviour
{
    public RobotInput.OnFootActions onFoot;
    public bool isTeleop;
    public bool isFeeding;
    public bool isShooting;

    private RobotController m_controller;
    private RobotMotor m_motor;
    private RobotInput m_input;
    private RobotNoteInsert m_noteInsert;
    private Animator m_animator;

    private void Start()
    {
        m_input = new RobotInput();
        onFoot = m_input.OnFoot;

        m_controller = GetComponent<RobotController>();
        m_motor = GetComponent<RobotMotor>();
        m_noteInsert = GetComponent<RobotNoteInsert>();
        m_animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (isTeleop)
        {
            if (isFeeding && !isShooting)
            {
                m_controller.Feed();
            }

            if (isShooting && !isFeeding)
            {
                m_controller.Shoot();
            }
        }

        if(m_animator != null)
        {
            if (m_noteInsert.isLoaded)
            {
                m_animator.enabled = true;
            }
            else
            {
                m_animator.Play("Kitbot_Shoot");
                m_animator.enabled = false;
            }

            m_animator.SetBool("isFeeding", isFeeding);
            m_animator.SetBool("isShooting", isShooting);
        }
    }

    private void FixedUpdate()
    {
        if (isTeleop)
        {
            m_motor.Steer(onFoot.Movement.ReadValue<Vector2>().x);
            m_motor.Accelerate(onFoot.Movement.ReadValue<Vector2>().y);
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