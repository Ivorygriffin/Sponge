using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public InputActionAsset input;
    InputAction movement;
    InputAction look;
    InputAction jump;
    InputAction boost;
    public CharacterController controller;

    [Space(15)]
    public float speed = 18f;
    public float boostSpeed = 24f;

    public float gravity = -10f;
    public float jumpHeight = 2f;
    public int jumpCount = 2;
    int jumpCounter;
    public float lookSpeed = 10f;

    bool jumpPressed = false;

    [Space(15)]
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    [Space(15)]
    public Transform model;
    public CameraMovement camera;
    public Animator animator;

    public Vector3 velocity;
    bool isGrounded;
    // Start is called before the first frame update
    void Start()
    {
        input.Enable();

        movement = input.FindAction("Movement");
        movement.Enable();

        look = input.FindAction("Look");
        look.Enable();

        jump = input.FindAction("Jump");
        jump.Enable();
        jump.performed += ctx => jumpPressed = true;
        jumpCounter = jumpCount;

        boost = input.FindAction("Boost");
        boost.Enable();

    }

    // Update is called once per frame
    void Update()
    {
        float x;
        float z;

        Vector2 delta = movement.ReadValue<Vector2>();
        x = delta.x;
        z = delta.y;
        Vector3 move = transform.right * x + transform.forward * z;

        move = camera.moveRotation * move;

        //transform.Rotate(new Vector3(0, movement.ReadValue<Vector2>().x * lookSpeed * Time.deltaTime, 0), Space.World);
        //z = movement.ReadValue<Vector2>().y;
        //Vector3 move = transform.forward * z;
        float boostPressed = boost.ReadValue<float>();


        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
            jumpCounter = jumpCount;
        }

        animator.SetFloat("Blend", move.magnitude);
        controller.Move(move * (boostPressed > 0.7f ? boostSpeed : speed) * Time.deltaTime);

        if (jumpPressed && (jumpCounter > 0 || isGrounded))
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            jumpCounter--;
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);


        if (move != Vector3.zero)
            model.forward = move.normalized;


        jumpPressed = false;
    }
}
