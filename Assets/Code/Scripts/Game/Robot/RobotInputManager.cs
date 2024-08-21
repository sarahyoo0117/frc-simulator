using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RobotInputManager : MonoBehaviourPunCallbacks
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
    private Camera m_camera;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        m_input = new RobotInput();
        onFoot = m_input.OnFoot;

        m_controller = GetComponent<RobotController>();
        m_motor = GetComponent<RobotMotor>();
        m_noteInsert = GetComponent<RobotNoteInsert>();
        m_animator = GetComponent<Animator>();
        m_camera = GetComponentInChildren<Camera>();

        m_camera.enabled = false;
    }

    private void Update()
    {
        if (!photonView.IsMine) return;

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
            m_animator.SetBool("isFeeding", isFeeding);
            m_animator.SetBool("isShooting", isShooting);
        }
    }

    private void FixedUpdate()
    {
        if (!photonView.IsMine) return;

        if (isTeleop)
        {
            m_motor.Steer(onFoot.Movement.ReadValue<Vector2>().x);
            m_motor.Accelerate(onFoot.Movement.ReadValue<Vector2>().y);
        }
    }

    public override void OnEnable()
    {
        if (!photonView.IsMine) return;
        onFoot.Enable();

        onFoot.Feed.started += ctx => isFeeding = true;
        onFoot.Feed.canceled += ctx => isFeeding = false;
        onFoot.Shoot.started += ctx => isShooting = true;
        onFoot.Shoot.canceled += ctx => isShooting = false;
    }

    public override void OnDisable()
    {
        if (!photonView.IsMine) return;
        onFoot.Disable();
    }
}