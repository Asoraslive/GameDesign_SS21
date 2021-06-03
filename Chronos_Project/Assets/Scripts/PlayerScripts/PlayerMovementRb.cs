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
    [SerializeField] float wallSpeed = 10f;

    [Header("Sprinting")]
    [SerializeField] float walkSpeed = 4f;
    [SerializeField] float sprintSpeed = 6f;
    [SerializeField] float acceleration = 10f;

    [Header("Jumping")]
    public float jumpforce = 5f;
    bool jumped;

    [Header("Drag")]
    [SerializeField] float groundDrag = 6f;
    [SerializeField] float airDrag = 2f;
    [SerializeField] float WallDrag = 2f;
    [SerializeField] float gravity=14.0f;
    [SerializeField] float verticalVelocity;
    [SerializeField] Vector3 gravityMove;

    float horizontalMovement;
    float verticalMovement;

    [Header("Grounded")]
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundMask;
    [SerializeField] bool isGrounded;
    float groundDistance = 0.4f;

    [Header("Keybinds")]
    [SerializeField] KeyCode sprintKey = KeyCode.LeftShift;

    Vector3 moveDirection;
    Vector3 slopeMoveDirection;

    [Header("References")]
    Rigidbody rb;
    [SerializeField] WallRun wallRunScript;
    [SerializeField] WaterControl waterControlScript;
    [SerializeField] Animator animator;
    [SerializeField] RaycastHit leftWallHit;
    [SerializeField] RaycastHit rightWallHit;

    [Header("Animator")]
    private int JumpHash = Animator.StringToHash("Jump");
    private int MidAirHash = Animator.StringToHash("MidAir");

    RaycastHit slopeHit;

    //slope Detection
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

        leftWallHit = wallRunScript.leftWallHit;
        rightWallHit = wallRunScript.rightWallHit;
        isGrounded = Physics.CheckSphere(groundCheck.position,groundDistance,groundMask);
        GravityHandler();
        MyInput();
        ControlDrag();
        ControlSpeed();

        if (Input.GetButtonDown("Jump") && isGrounded )
        {
            Jump();
            jumped = false;
            animator.SetTrigger(JumpHash);
            if (isGrounded)
            {
                animator.SetBool(MidAirHash, false);
            }
            else if (!isGrounded)
            {
                animator.SetBool(MidAirHash, true);
            }
        }
        if (isGrounded)
        {
            if(jumped == true)
            {
                jumped = false;
            }
            animator.SetBool(MidAirHash, false);
        }
        else if (!isGrounded)
        {
            animator.SetBool(MidAirHash, true);
        }

        slopeMoveDirection = Vector3.ProjectOnPlane(moveDirection, slopeHit.normal);
    }

    void GravityHandler()
    {
        if (isGrounded) // regular gravity while on ground
        {
            verticalVelocity = -gravity*Time.deltaTime;

        }
        else if (wallRunScript.currentlyWallrunning) //gravity while wallRunning
        {
            verticalVelocity = 0f;
        }
        else if (waterControlScript.isInWater)//swim gravity
        {
            verticalVelocity = 0f;
        }
        else
        { // Fall gravity capped
            if (verticalVelocity < (2*-gravity))
            {
                verticalVelocity = (2*-gravity);
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
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);

            rb.AddForce(transform.up * jumpforce, ForceMode.Impulse);


    }

    void MyInput() //basic movement
    {
        horizontalMovement = Input.GetAxis("Horizontal");
        verticalMovement = Input.GetAxis("Vertical");

        moveDirection = orientation.forward * verticalMovement + orientation.right * horizontalMovement;
    }

    void ControlSpeed() //Sprint
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

    void ControlDrag() //Drag Control
    {
        if (isGrounded)
        {
            rb.drag = groundDrag;
        }
        else if(wallRunScript.currentlyWallrunning)
        {
            rb.drag = WallDrag;
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

    void MovePlayer() //Moving the player
    {
        if ((isGrounded && !OnSlope()))
        {
        rb.AddForce(moveDirection.normalized * moveSpeed *movementMultiplier, ForceMode.Acceleration);

        }
        else if (isGrounded && OnSlope())
        {
        rb.AddForce(slopeMoveDirection.normalized * moveSpeed * movementMultiplier, ForceMode.Acceleration);
        } 
        else if (wallRunScript.currentlyWallrunning)
        {
            //left wall dir


            //rightwall dir

            //move 
        rb.AddForce(orientation.forward * wallSpeed * movementMultiplier, ForceMode.Acceleration);
        }
        else
        {
        rb.AddForce(moveDirection.normalized * moveSpeed *movementMultiplier*airMultiplier, ForceMode.Acceleration);
        }
    }
}
