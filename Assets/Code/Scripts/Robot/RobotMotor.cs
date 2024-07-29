using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RobotMotor : MonoBehaviour
{
    public float speed = 7f;
    public float rotateSpeed = 280f;
    public float mass = 5f;
    public float gravity = 9.8f;

    private CharacterController controller;
    private Vector3 robotVelocity;
    private bool isGrounded;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (controller != null && controller.enabled == true)
        {
            isGrounded = controller.isGrounded;
        }
    }

    public void ProcessMove(Vector2 input)
    {
        if (controller != null && controller.enabled == true)
        {
            Vector3 moveDirection = Vector3.zero;
            moveDirection.x = input.x;
            moveDirection.z = input.y;
            controller.Move(transform.TransformDirection(moveDirection) * speed * Time.deltaTime);

            robotVelocity.y -= mass * gravity * Time.deltaTime;

            if (isGrounded && robotVelocity.y < 0)
                robotVelocity.y = -2f;

            controller.Move(robotVelocity * Time.deltaTime);
        }
    }

    public void ProcessRotation(Vector2 input)
    {
        transform.Rotate(Vector3.up * input.x * rotateSpeed * Time.deltaTime);
    }

    public void Shoot()
    {
        print("Shooted");
    }

    public void Feed()
    {
        print("feeding");
    }
}
