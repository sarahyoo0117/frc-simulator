using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    public float speed = 5f;
    public float mass = 5f;
    public float gravity = 9.8f;
    public float jumpHeight = 10f;

    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool isGrounded;

    private bool isCrouching;
    private float crouchTimer;
    private bool lerpCrouch;

    private bool isSprinting;

    void Start()
    {
        controller = GetComponent<CharacterController>();  
    }

    void Update()
    {
        isGrounded = controller.isGrounded; //TODO: fix or create my own

        if (lerpCrouch)
        {
            crouchTimer += Time.deltaTime;
            float p = crouchTimer / 1;
            p *= p;
            if (isCrouching)
                controller.height = Mathf.Lerp(controller.height, 1, p);
            else
                controller.height = Mathf.Lerp(controller.height, 2, p);

            if (p > 1)
            {
                lerpCrouch = false;
                crouchTimer = 0f;
            }
        }
        
    }

    public void ProcessMove(Vector2 input)
    {
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = input.x;
        moveDirection.z = input.y;
        controller.Move(transform.TransformDirection(moveDirection) * speed * Time.deltaTime);
        playerVelocity.y -= mass * gravity * Time.deltaTime;

        if (isGrounded && playerVelocity.y < 0)
            playerVelocity.y = -2f;

         controller.Move(playerVelocity * Time.deltaTime);
    }

    public void Jump()
    {
        if (isGrounded)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * 3.0f * gravity);
        }
    }

    public void Crouch()
    {
        isCrouching = !isCrouching;
        crouchTimer = 0;
        lerpCrouch = true;
        if (isCrouching)
            speed = 5f;
    }

    public void Sprint()
    {
        isSprinting = !isSprinting;
        if (isSprinting && !isCrouching)
            speed = 10f;
        else
            speed = 5f;
    }
}
