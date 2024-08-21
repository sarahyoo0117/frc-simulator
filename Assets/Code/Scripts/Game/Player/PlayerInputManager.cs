using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerInputManager : MonoBehaviourPunCallbacks
{
    public PlayerInput.OnFootActions onFoot;

    private PlayerMotor motor;
    private PlayerLook look;
    private PlayerInput playerInput;
    private CharacterController m_controller;

    private void Awake()
    {
        playerInput = new PlayerInput();
        onFoot = playerInput.OnFoot;

        motor = GetComponent<PlayerMotor>();
        look = GetComponent<PlayerLook>();
        m_controller = GetComponent<CharacterController>();

        onFoot.Jump.performed += ctx => motor.Jump();
        onFoot.Crouch.performed += ctx => motor.Crouch();
        onFoot.Sprint.performed += ctx => motor.Sprint();
    }

    private void FixedUpdate()
    {
        if(photonView.IsMine)
        {
            motor.ProcessMove(onFoot.Movement.ReadValue<Vector2>());
        }
    }

    private void LateUpdate()
    {
        if (photonView.IsMine && m_controller.enabled)
        {
            look.ProcessLook(onFoot.Look.ReadValue<Vector2>());
        }
    }

    public override void OnEnable()
    {
        onFoot.Enable();
    }

    public override void OnDisable()
    {
        onFoot.Disable();
    }
}
