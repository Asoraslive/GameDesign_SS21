using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementRb : MonoBehaviour
{
    float playerHeight = 2f;

    [SerializeField] Transform orientation;

    [Header("Movement")]
    public float moveSpeed = 6f;
    float movementMultiplier = 10f;
    [SerializeField] float airMultiplier = 0.4f;

    [Header("Sprinting")]
    [SerializeField] float walkSpeed = 4f;
    [SerializeField] float sprintSpeed = 6f;
    [SerializeField] float acceleration = 10f;

    [Header("Jumping")]
    public float jumpforce = 5f;

    [Header("Drag")]
    [SerializeField] float groundDrag = 6f;
    [SerializeField] float airDrag = 2f;
    [SerializeField] float gravity=14.0f;
    [SerializeField] float verticalVelocity;
    [SerializeField] Vector3 gravityMove;

    float horizontalMovement;
    float verticalMovement;

    [Header("Grounded")]
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundMask;
    bool isGrounded;
    float groundDistance = 0.4f;

    [Header("Keybinds")]
    [SerializeField] KeyCode sprintKey = KeyCode.LeftShift;

    Vector3 moveDirection;
    Vector3 slopeMoveDirection;

    [Header("References")]
    Rigidbody rb;
    [SerializeField] WallRun wallRunScript;

    RaycastHit slopeHit;

    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight / 2 + .5f))
        {
            if (slopeHit.normal != Vector3.up)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position,groundDistance,groundMask);
        GravityHandler();
        MyInput();
        ControlDrag();
        ControlSpeed();

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }

        slopeMoveDirection = Vector3.ProjectOnPlane(moveDirection, slopeHit.normal);
    }

    void GravityHandler()
    {
        if (isGrounded)
        {
            verticalVelocity = -gravity*Time.deltaTime;

        }
        else if (wallRunScript.currentlyWallrunning)
        {
            verticalVelocity = 0f;
        }
        else
        {
            if (verticalVelocity < -14f)
            {
                verticalVelocity = -14f;
            }
            else
            {
                verticalVelocity -= gravity * Time.deltaTime;
            }
        }

        gravityMove = new Vector3(0f, verticalVelocity, 0f);
        rb.AddForce(gravityMove);
    }

    void Jump()
    {
        if (isGrounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(transform.up * jumpforce, ForceMode.Impulse);
        }
    }

    void MyInput()
    {
        horizontalMovement = Input.GetAxis("Horizontal");
        verticalMovement = Input.GetAxis("Vertical");

        moveDirection = orientation.forward * verticalMovement + orientation.right * horizontalMovement;
    }

    void ControlSpeed()
    {
        if(Input.GetKey(sprintKey) && isGrounded)
        {
            moveSpeed = Mathf.Lerp(moveSpeed, sprintSpeed, acceleration * Time.deltaTime);
        }
        else
        {
            moveSpeed = Mathf.Lerp(moveSpeed, walkSpeed, acceleration * Time.deltaTime);
        }
    }

    void ControlDrag()
    {
        if (isGrounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = airDrag;
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        if (isGrounded && !OnSlope())
        {
        rb.AddForce(moveDirection.normalized * moveSpeed *movementMultiplier, ForceMode.Acceleration);

        }
        else if (isGrounded && OnSlope())
        {
        rb.AddForce(slopeMoveDirection.normalized * moveSpeed * movementMultiplier, ForceMode.Acceleration);
        }
        else
        {
        rb.AddForce(moveDirection.normalized * moveSpeed *movementMultiplier*airMultiplier, ForceMode.Acceleration);
        }
    }
}
