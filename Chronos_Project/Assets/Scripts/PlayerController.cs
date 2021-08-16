using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerController : MonoBehaviour
{
    /*  Variables   */
    [Header("References")]
    [SerializeField] Rigidbody rbPlayer;
    [SerializeField] GameObject orientation;
    [SerializeField] DetectObj groundDetect;
    [SerializeField] DetectObj WallDetect;
    [SerializeField] AnimationStateController animationStateController;
    [SerializeField] CinemachineVirtualCamera cam;
    [SerializeField] PauseMenu pauseMenuScript;
    [SerializeField] Tool_Tips tooltipsScript;



    /*Camera*/
    [Header("Camera Stuff")]
    [SerializeField] float xRotation;
    [SerializeField] float sensitivity = 50f;
    [SerializeField] float sensMultiplier = 1f;
    [SerializeField] float fov = 70f;
    [SerializeField] float wallrunFovChanger = 30f;
    [SerializeField] float fovChangeTime = 5f;
    [SerializeField] float tilt = 20f;
    [SerializeField] float mouseX;
    [SerializeField] float mouseY;
    [SerializeField] float mouseXSens;
    [SerializeField] float mouseYSens;






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

    [Header("Water Movement")]
    [SerializeField] bool swimming;
    [SerializeField] float waterdrag = 0f;
    [SerializeField] float water_upforce = 5f;
    private bool left_water = false;


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
    [SerializeField] float wallrunCooldownTime = 3f;
    [SerializeField] float wallrunEndCooldownTime = .3f;

    private IEnumerator wallrunCd;
    [SerializeField] bool wallrunCdCRActive;

    [Header("ArchingParameters")]
    [SerializeField] float WallrunUpforce;
    [SerializeField] float WallrunUpforce_DecreaseRate;
    private float upforce;

    [Header("Godmode Parameters")]
    [SerializeField] float godModeUpForce = 10f;
    [SerializeField] float godModeDownForce = 10f;

    [Header("Sounds")]
    [SerializeField] bool walksound = false;
    [SerializeField] bool wallrunsound = false;
    [SerializeField] bool swimsound = false;
    [SerializeField] AudioSource audio_walk;
    [SerializeField] AudioSource audio_wallrun;
    [SerializeField] AudioSource audio_swmimming;
    [SerializeField] AudioSource audio_falldown;
    [SerializeField] AudioSource audio_water_falldown;
    


        /*  Awake,Start,Update,FixedUpdate    */
        private void Awake()
        {

        }
        private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        floating = isFloating();
        cam.m_Lens.FieldOfView = fov;
    }
    private void Update()
    {
        moveDir = playerInputHandle();
        if ((grounded || djumping) && Input.GetKeyDown(KeyCode.Space) && (jumpingCooldown == false) && !isWallRunning) Jump();
        else if (jumpingCooldown == true && Input.GetKeyDown(KeyCode.Space)) Debug.Log("Jump on Cooldown");
        else if (isWallRunning && Input.GetKeyDown(KeyCode.Space)) wallJump();


        if (checkWallrun() && Input.GetKey(KeyCode.Space) && !isWallRunning && !grounded) startWallrun();
        else if ((checkWallrun() == false || Input.GetKeyUp(KeyCode.Space)) && isWallRunning) stopWallrun();

        // Sound Abfragen
        if(grounded && !swimming && rbPlayer.velocity.magnitude > 0.2f && !audio_walk.isPlaying && !left_water) {     // Walking Sound
            audio_walk.Play();
        }
    }

    private void FixedUpdate()
    {
        if (!godMode)
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
        if (!isWallRunning) { movePlayer(moveDir); }
        else if (isWallRunning) RunWall();

        if (!isWallRunning && wallrunCdCRActive == false)
        {
            cam.m_Lens.FieldOfView = Mathf.Lerp(cam.m_Lens.FieldOfView, fov, fovChangeTime * Time.deltaTime);
        }
    }

    private void LateUpdate()
    {
       // if (pauseMenuScript.getPause() == false) Look(mouseX, mouseY);
    }

    /*  Simple Movement */

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Water")) {
            swimming = true;
            gravity = false;
            // Slashdown Sound
            StartCoroutine("startSwimming");

            // Starte Swimming Sound--------------------------------------------------------------------------------------------------------
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Water")) {
            swimming = false;
            gravity = true;
            // Beende Swimming Sound--------------------------------------------------------------------------------------------------------
            audio_swmimming.Stop();
            StartCoroutine("waterLeft");
        }
    }

    /*  Get Player Input */
    public Vector3 playerInputHandle() 
    {
        //mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.fixedDeltaTime * sensMultiplier;
        //mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.fixedDeltaTime * sensMultiplier;

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
        if (jumping || swimming) return;

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

        if (!grounded) {
            djumping = false;
            // Deactivate Tool Tip: DoubleJump
            tooltipsScript.Tip_doublejump(false);
        }

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
            // Wall Run Sound starten---------------------------------------------------------------------------------------------------------------
            if (!left_water)
            {
                audio_wallrun.Play();
            }

            isWallRunning = true;
            gravity = false;
            upforce = WallrunUpforce;
            djumping = true;
            currentWall = WallDetect.colnow;

            // Deactivate Tool Tip: Wallride
            tooltipsScript.Tip_wallride(false);

        }
    }

    /*Stop wallrun*/
    private void stopWallrun()
    {
        //Beende Wall Run Sound-------------------------------------------------------------------------------------------------------------------
        audio_wallrun.Stop();

        if (wallrunCdCRActive) {
            Debug.Log("Force Shut");
            StopCoroutine(wallrunCd);
            wallrunCdCRActive = false;        
        }

        StartCoroutine(wallrunEndCooldown());
    }

    /*WallrunEndCooldown*/
    private IEnumerator wallrunEndCooldown()
    {
        yield return new WaitForSeconds(wallrunEndCooldownTime);
        isWallRunning = false;
        gravity = true;
        Debug.Log("End normal");
    }


    /*Set WallJump Parameters*/
    private void wallJump()
    {
            rbPlayer.velocity = transform.forward * wallJumpForwardVelocity + transform.up * wallJumpUpVelocity;
            //rbPlayer.AddForce(transform.forward * wallJumpForwardVelocity + transform.up * wallJumpUpVelocity , ForceMode.Impulse);
            Debug.Log("Walljumped");
            StartCoroutine(jumpCooldown());
            isWallRunning = false;
            gravity = true;


    }

    /*Runwall movement*/
    private void RunWall()
    {
        //lerp Fov
        cam.m_Lens.FieldOfView = Mathf.Lerp(cam.m_Lens.FieldOfView, fov+wallrunFovChanger, fovChangeTime*Time.deltaTime);

        //stick to wall
        rbPlayer.AddForce(-WallDetect.normalOfColliding*Time.deltaTime);

        //arch
        rbPlayer.velocity = new Vector3(rbPlayer.velocity.x, upforce, rbPlayer.velocity.z);
        upforce -= WallrunUpforce_DecreaseRate * Time.deltaTime;



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
        yield return new WaitForSeconds(wallrunCooldownTime);
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


    /*Camera Stuff*/
    private float desiredX;
    private void Look(float x,float y)
    {
        //Find current look rotation
        Vector3 rot = cam.transform.localRotation.eulerAngles;
        desiredX = rot.y + x;

        //Rotate, and also make sure we dont over- or under-rotate.
        xRotation -= y;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        //Perform the rotations
        cam.transform.localRotation = Quaternion.Euler(xRotation, desiredX, 0);
    }



    /*Setter / Getter */
    public void setFov(float newFov)
    {
        fov = newFov;
        cam.m_Lens.FieldOfView = fov;
    }
    public float getFov() { return fov; }

    IEnumerator startSwimming()
    {
        audio_water_falldown.time = 0.4f;
        audio_water_falldown.Play();
        yield return new WaitForSeconds(1f);
        audio_water_falldown.Stop();
        audio_swmimming.Play();
    }

    IEnumerator waterLeft()
    {
        left_water = true;
        yield return new WaitForSeconds(0.3f);
        left_water = false;
    }
}
