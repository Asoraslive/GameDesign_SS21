using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRun : MonoBehaviour
{
    [SerializeField] PlayerMovementRb moveScript;

    [Header("Movement")]
    [SerializeField] Transform orientation;
    [SerializeField] float minimumJumpHeight = 1.5f;


    [Header("Detection")]
    [SerializeField] float wallDistance = .5f;
    [SerializeField] LayerMask wallMask;


    [Header("Wall Running")]
    [SerializeField] private float wallRunGravity;
    [SerializeField] private float wallRunJumpForce;
    [SerializeField] bool wallruns;
    [SerializeField] float wallJumpRangeMultiplier = 2f;
    public bool currentlyWallrunning { get; set; }


    [Header("Camera")]
    [SerializeField] private Camera cam;
    [SerializeField] private float fov;
    [SerializeField] private float wallRunfov;
    [SerializeField] private float wallRunfovTime;
    [SerializeField] private float camTilt;
    [SerializeField] private float camTiltTime;

    public float tilt { get; private set; }
    
    bool wallLeft = false;
    bool wallRight = false;

    public RaycastHit leftWallHit;
    public RaycastHit rightWallHit;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    bool CanWallRun()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minimumJumpHeight);
    }

    void CheckWall()
    {
        wallLeft = Physics.Raycast(transform.position, -orientation.right,out leftWallHit, wallDistance,wallMask);
        wallRight = Physics.Raycast(transform.position, orientation.right,out rightWallHit, wallDistance,wallMask);

    }

    private void Update()
    {
        if (currentlyWallrunning)
        {
            wallruns = true;
        }
        else
        {
            wallruns = false;
        }
        CheckWall();
        Vector3 test = rb.velocity;
        test.y = 0;
        if (CanWallRun() &&  test.magnitude > 2f && !moveScript.isGrounded)
        {
            if (wallLeft)
            {
                StartWallRun();
            }
            else if (wallRight)
            {
                StartWallRun();
            }
            else if (rb.velocity.magnitude < 4f)
            {
                StopWallRun();
            }
            else
            {
                StopWallRun();
            }
        }
        else
        {
            StopWallRun();
        }
    }

   void StartWallRun()
    {
        currentlyWallrunning = true;
        rb.AddForce(Vector3.down * wallRunGravity, ForceMode.Force);

        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, wallRunfov, wallRunfovTime*Time.deltaTime);
        if(wallLeft)
        {
            tilt = Mathf.Lerp(tilt, -camTilt, camTiltTime * Time.deltaTime);
        }
        else if (wallRight)
        {
            tilt = Mathf.Lerp(tilt, camTilt, camTiltTime * Time.deltaTime);
        }



        if (Input.GetButtonDown("Jump"))
        {
            if (wallLeft && Input.GetKey(KeyCode.D))
            {
                Vector3 walljumpDirection = transform.up + leftWallHit.normal * wallJumpRangeMultiplier;
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
                rb.AddForce(walljumpDirection * wallRunJumpForce*100, ForceMode.Force);
            }
            else if (wallRight && Input.GetKey(KeyCode.A))
            {
                Vector3 walljumpDirection = transform.up + rightWallHit.normal* wallJumpRangeMultiplier;
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
                rb.AddForce(walljumpDirection * wallRunJumpForce*100, ForceMode.Force);
            }
            else if (wallLeft)
            {
                Vector3 walljumpDirection = transform.up + leftWallHit.normal ;
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
                rb.AddForce(walljumpDirection * wallRunJumpForce*100, ForceMode.Force);
            }
            else if (wallRight)
            {
                Vector3 walljumpDirection = transform.up + rightWallHit.normal;
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
                rb.AddForce(walljumpDirection * wallRunJumpForce*100, ForceMode.Force);
            }
        }
    }

    void StopWallRun()
    {
        currentlyWallrunning = false;
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, fov, wallRunfovTime * Time.deltaTime);
        tilt = Mathf.Lerp(tilt, 0, camTiltTime * Time.deltaTime);
    }
}
