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
    private RobotDrive m_drive;
    private RobotInput m_input;
    private Animator m_animator;
    private Camera m_camera;

    Vector2 leftStick, rightStick;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        m_input = new RobotInput();
        onFoot = m_input.OnFoot;

        m_controller = GetComponent<RobotController>();
        m_drive = GetComponent<RobotDrive>();
        m_animator = GetComponent<Animator>();
        m_camera = GetComponentInChildren<Camera>();

        m_camera.enabled = false;
    }

    private void Update()
    {
        if (!photonView.IsMine) return;

        if (isTeleop)
        {
            if (isFeeding)
            {
                m_controller.Feed();
            }
            if (isShooting)
            {
                m_controller.Shoot();
            }
        }
    }

    private void FixedUpdate()
    {
        if (!photonView.IsMine) return;

        if (isTeleop)
        {
            //TODO: Sync animations
            m_animator.SetBool("isFeeding", isFeeding);
            m_animator.SetBool("isShooting", isShooting);

            leftStick = onFoot.Movement.ReadValue<Vector2>();
            rightStick = onFoot.Look.ReadValue<Vector2>();

            m_drive.Steer(leftStick, rightStick);
            m_drive.Rotate(rightStick.x); //TODO: fix robot rotation
            m_drive.Accelerate(leftStick, rightStick);
        }
    }

    public override void OnEnable()
    {
        if (!photonView.IsMine) return;
        m_input.Enable();

        onFoot.Feed.started += ctx => isFeeding = true;
        onFoot.Feed.canceled += ctx => isFeeding = false;
        onFoot.Shoot.started += ctx => isShooting = true;
        onFoot.Shoot.canceled += ctx =>
        {
            isShooting = false;
            m_controller.readyToShoot = false;
        };
    }

    public override void OnDisable()
    {
        if (!photonView.IsMine) return;
        m_input.Disable();
    }
}