using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    public CharacterController controller;
    [SerializeField]
    public Rigidbody rb;
    [SerializeField]
    public float speed = 12f;
    [SerializeField]
    public float gravity = 5f;
    [SerializeField]
    public float height = 2f;

    bool isGrounded;
    float moveY;


    [SerializeField]
    public Transform groundCheck;
    [SerializeField]
    public float groundDistance = 0.4f;
    [SerializeField]
    public LayerMask groundMask;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if(isGrounded)
        {
            moveY = -2f;
        }


        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        if (Input.GetButtonDown("Jump"))
        {
            Debug.Log("Space has been pressed");
            moveY = height;
        }

        moveY -= gravity * Time.deltaTime;

        move.y = moveY;
        controller.Move(move * speed*Time.deltaTime);
    }

}
