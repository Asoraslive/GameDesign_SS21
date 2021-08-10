using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    /*  Variables   */
    [Header("References")]
    [SerializeField] Rigidbody rbPlayer;
    [SerializeField] GameObject orientation;
    [SerializeField] DetectObj groundDetect;
    [SerializeField] DetectObj WallDetect;
    [SerializeField] AnimationStateController animationStateController;




    [Header("Simple Movement")]
    [SerializeField] Vector3 moveDir;
    [SerializeField] float moveSpeed = 4500f;
    [SerializeField] float groundDrag = .5f;
    [SerializeField] float counterMovement = .175f;
    [SerializeField] float maxSpeed = 20;

    [Header("Air Movement")]
    [SerializeField] bool floating;
    [SerializeField] float airDrag = .3f;
    [SerializeField] float gravityForce = 30f;
    [SerializeField] bool gravity = true;


    private float threshold = .01f;

    [Header("Jumping")]
    [SerializeField] float jumpForce = 550f;
    [SerializeField] bool jumping = false;
    [SerializeField] bool jumpingCooldown = false;
    [SerializeField] float jumpingCooldownTime = .3f;
    [SerializeField] bool djumping = true;
    [SerializeField] bool grounded;

    [Header("Wallrun")]
    [SerializeField] Vector3 wallMoveDir;
    [SerializeField] float wallJumpForwardVelocity = 1f;
    [SerializeField] float wallJumpUpVelocity = 10f;
    [SerializeField] bool wallTouch;
    [SerializeField] bool isWallRunning;
    [SerializeField] RaycastHit wallHit;
    [SerializeField] Vector3 wallSpot;
    [SerializeField] Collider currentWall;
    [SerializeField] Collider newWall;
    [SerializeField] float wallDrag = .1f;

    private IEnumerator wallrunCd;
    private bool wallrunCdCRActive;

    [Header("ArchingParameters")]
    [SerializeField] float WallrunUpforce;
    [SerializeField] float WallrunUpforce_DecreaseRate;
    private float upforce;

    [Header("Godmode Parameters")]
    [SerializeField] float godModeUpForce = 10f;
    [SerializeField] float godModeDownForce = 10f;



    /*  Awake,Start,Update,FixedUpdate    */
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        floating = isFloating();
    }
    private void Update()
    {
        moveDir = playerInputHandle();
        if ((grounded || djumping) && Input.GetKeyDown(KeyCode.Space) && (jumpingCooldown == false) && !isWallRunning) Jump();
        else if (jumpingCooldown == true && Input.GetKeyDown(KeyCode.Space)) Debug.Log("Jump on Cooldown");

        if (checkWallrun() && Input.GetKey(KeyCode.Space) && !isWallRunning && !grounded) startWallrun();
        else if ((checkWallrun() == false || Input.GetKeyUp(KeyCode.Space)) && isWallRunning) stopWallrun();
    }

    private void FixedUpdate()
    {
        if(!godMode)
        checkAllColliders();

        //Handle Drag
        if (grounded) 
        {
            currentWall = null;
            DragHandler(groundDrag); 
        }
        else if (floating && !isWallRunning) DragHandler(airDrag);
        else if (isWallRunning) DragHandler(wallDrag);

        if (grounded) djumping = true;


        RotatePlayer();


        if (Input.GetKeyDown(KeyCode.G)) { godMode = !godMode; if (godMode == true) { gravity = false; } else gravity = true; }
        GodMode();
        if (!isWallRunning) movePlayer(moveDir);
        else if (isWallRunning) RunWall();
        
    }


    /*  Simple Movement */

    /*  Get Player Input */
    public Vector3 playerInputHandle() 
    {
        jumping = Input.GetKeyDown(KeyCode.Space);
        return new Vector3(Input.GetAxisRaw("Horizontal"),0,Input.GetAxisRaw("Vertical"));
    }


    /*Move rb according to moev Dir*/
    public void movePlayer(Vector3 dir) 
    {

        //gravity
        if(gravity == true)
        {
        rbPlayer.AddForce(Vector3.down * Time.deltaTime * gravityForce);
        }

        //actual Velocity relative to Cam
        Vector2 mag = FindRelVelToCam();
        float xMag = mag.x, yMag = mag.y;

        //countermovement on ground
        CounterMovement(dir.x, dir.z, mag);


        //set Max Speed
        float maxSpeed = this.maxSpeed;

        //if speed is larger than maxSpeed cancel input so it doesnt go over max
        if (dir.x > 0 && xMag > maxSpeed) dir.x = 0;
        if (dir.x < 0 && xMag < -maxSpeed) dir.x = 0;
        if (dir.z > 0 && yMag > maxSpeed) dir.z = 0;
        if (dir.z < 0 && yMag < -maxSpeed) dir.z = 0;

        //slight multiplier for movement(more like halfing for air movement)
        float multiplier = 1f, multiplierV = 1f;


        //applying force to player
        //Debug.Log("orientation Movement : " + orientation.transform.forward);
        rbPlayer.AddForce(orientation.transform.forward * dir.z * moveSpeed * Time.deltaTime * multiplier * multiplierV);
        rbPlayer.AddForce(orientation.transform.right * dir.x * moveSpeed * Time.deltaTime * multiplier);
        

    }

    /*CounterMovement*/
    private void CounterMovement(float x, float y, Vector2 mag)
    {
        //No counterMovement (TO-DO if gorunded)
        if (jumping) return;

        //Counter movement
        if (Mathf.Abs(mag.x) > threshold && Mathf.Abs(x) < 0.05f || (mag.x < -threshold && x > 0) || (mag.x > threshold && x < 0))
        {
            rbPlayer.AddForce(moveSpeed * orientation.transform.right * Time.deltaTime * -mag.x * counterMovement);
        }
        if (Mathf.Abs(mag.y) > threshold && Mathf.Abs(y) < 0.05f || (mag.y < -threshold && y > 0) || (mag.y > threshold && y < 0))
        {
            rbPlayer.AddForce(moveSpeed * orientation.transform.forward * Time.deltaTime * -mag.y * counterMovement);
        }
    }

    /*Get Velocity relativ to where the cam is looking towards*/
    public Vector2 FindRelVelToCam()
    {
        float lookAngle = orientation.transform.eulerAngles.y;
        float moveAngle = Mathf.Atan2(rbPlayer.velocity.x, rbPlayer.velocity.z) * Mathf.Rad2Deg;

        float u = Mathf.DeltaAngle(lookAngle, moveAngle);
        float v = 90 - u;

        float magnitude = rbPlayer.velocity.magnitude;
        float yMag = magnitude * Mathf.Cos(u * Mathf.Deg2Rad);
        float xMag = magnitude * Mathf.Cos(v * Mathf.Deg2Rad);

        return new Vector2(xMag,yMag);
    }

    /*Rotating player towards certain dir*/
    private void RotatePlayer()
    {
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x,Camera.main.transform.localEulerAngles.y,transform.localEulerAngles.z);
    }

    /*Drag Handler Changes drag base on current situation*/
    private void DragHandler(float drag) => rbPlayer.drag = drag;

    /*      */
    /* Jump */
    /*      */

    private void Jump()
    {
        Debug.Log("Djump = " + grounded);
        //add JumpForce

        rbPlayer.velocity += new Vector3(0,jumpForce,0);
        StartCoroutine(jumpCooldown());
        //If jumping while falling, reset y velocity.
        Vector3 vel = rbPlayer.velocity;
        if (rbPlayer.velocity.y < 0.5f)
            rbPlayer.velocity = new Vector3(vel.x, 0, vel.z);
        else if (rbPlayer.velocity.y > 0)
            rbPlayer.velocity = new Vector3(vel.x, vel.y / 2, vel.z);
        
        if (!grounded) djumping = false;

        //play animation
        animationStateController.TriggerJump();
    }

    /*JumpCooldown*/
    IEnumerator jumpCooldown()
    {
        jumpingCooldown = true;
        yield return new WaitForSeconds(jumpingCooldownTime);
        jumpingCooldown = false;
    }

    /* Ground Detect */
    private bool isGrounded() { return groundDetect.Obstructed;  } 

    private bool isWall() { return WallDetect.Obstructed;  }

    /*Bool check if is in Air*/
    public bool isFloating() { return groundDetect.Obstructed == false ? true : false; }

    /*Checking all Colliders*/
    private void checkAllColliders() 
    {
        grounded = isGrounded();
        floating = isFloating();
        wallTouch = isWall();
    }


    /*Check if wallrun possible/available */
    private bool checkWallrun() { return wallTouch; }
    /* Calc wall run dir*/
    private Vector3 calcWallRunDir(Vector3 wallHit,Vector3 playerDir)
    {
        Vector3 res = Vector3.ProjectOnPlane(playerDir, wallHit);
        Debug.Log(res);
        return res;
    }

    /*       */
    /*Wallrun*/
    /*       */
    private void startWallrun()
    {
        if(currentWall == null ||  currentWall != WallDetect.colnow)
        {
            isWallRunning = true;
            gravity = false;
            upforce = WallrunUpforce;
            djumping = true;
            currentWall = WallDetect.colnow;
        }
    }

    /*Stop wallrun*/
    private void stopWallrun()
    {
        if (wallrunCdCRActive) {
            Debug.Log("Force Shut");
            StopCoroutine(wallrunCd); }

        StartCoroutine(wallrunEndCooldown());
    }

    /*WallrunEndCooldown*/
    private IEnumerator wallrunEndCooldown()
    {
        yield return new WaitForSeconds(.1f);
        isWallRunning = false;
        gravity = true;
        Debug.Log("End normal");
    }


    /*Set Wallrun Parameters*/
    private void wallRunUpdate()
    {
        //get Contact Point of wall

    }

    /*Runwall movement*/
    private void RunWall()
    {

        //stick to wall
        rbPlayer.AddForce(-WallDetect.normalOfColliding*Time.deltaTime);

        //wallRunUpdate
        wallRunUpdate();

        //arch
        rbPlayer.velocity = new Vector3(rbPlayer.velocity.x, upforce, rbPlayer.velocity.z);
        upforce -= WallrunUpforce_DecreaseRate * Time.deltaTime;

        //wallJump
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rbPlayer.velocity = transform.forward * wallJumpForwardVelocity + transform.up * wallJumpUpVelocity;
            Debug.Log("Walljumped");
            StartCoroutine(jumpCooldown());
            isWallRunning = false;
            gravity = true;
        }



        //3sec
        if(wallrunCdCRActive == false)
        { 
            wallrunCd = wallrunCooldown();
            StartCoroutine(wallrunCd);
        }


    }

    /*WallrunCooldown*/
    private IEnumerator wallrunCooldown()
    {
        Debug.Log("Started Routine");
        wallrunCdCRActive = true;
        yield return new WaitForSeconds(3f);
        wallrunCdCRActive = false;
        stopWallrun();
        
    }






    private bool godMode = false;

    /* GOD MODE*/
    private void GodMode()
    {
        if (godMode == true)
        {

            if (Input.GetKey(KeyCode.Space))
            {
                rbPlayer.AddForce(Vector3.up * Time.deltaTime*godModeUpForce);
            }
            if  (Input.GetKey(KeyCode.V))
            {
                rbPlayer.AddForce(Vector3.down * Time.deltaTime*godModeDownForce);
            }
        }
        
    }
}
