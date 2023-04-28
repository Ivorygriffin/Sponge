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

    [Tooltip("Acceleration due to gravity, added per second to y velocity")]
    public float gravity = -10f;
    [Tooltip("Velocity is set to this value while grounded")]
    public float cloudGravity = -2f;
    [Tooltip("Height in meters the player jumps")]
    public float jumpHeight = 2f;
    [Tooltip("Amount of jumps the player can use before touching the ground")]
    public int jumpCount = 2;
    int jumpCounter;
    public float lookSpeed = 10f;

    [Tooltip("Amount of water used by boost")]
    public float waterUseSpeed = 1;
    [Tooltip("Amount of water the player can store")]
    public float waterMax = 10f;
    public float collectableMax = 6f;
    float waterCount;
    float collectableCount;
    public float WaterCount
    {
        get { return waterCount; }
        set { waterCount = Mathf.Clamp(value, 0, waterMax); }
    }
    public float CollectableCount
    {
        get { return collectableCount; }
        set { collectableCount = Mathf.Clamp(value, 0, collectableMax); }
    } 
   

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


        waterCount = waterMax;
        collectableCount = collectableMax;
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
            velocity.y = cloudGravity;
            jumpCounter = jumpCount;
        }

        animator.SetFloat("Blend", move.magnitude);
        controller.Move(move * (boostPressed > 0.7f && waterCount > 0 ? boostSpeed : speed) * Time.deltaTime);

        if (jumpPressed && (jumpCounter > 0 || isGrounded))
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            jumpCounter--;
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);


        if (move != Vector3.zero)
        {
            model.forward = move.normalized;
            if (boostPressed > 0.7f && waterCount > 0)
                waterCount -= Time.deltaTime * waterUseSpeed;
        }

        UIManager.Instance.waterPercent = WaterCount / waterMax;
        UIManager.Instance.collectablePercent = CollectableCount / collectableMax;
        jumpPressed = false;
    }
}
