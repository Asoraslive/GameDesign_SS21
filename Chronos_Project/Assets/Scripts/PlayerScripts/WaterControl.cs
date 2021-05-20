using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterControl : MonoBehaviour
{
    [Header("References")]
    public bool isInWater = false;
    private Rigidbody _rb;

    [Header("Swimming")]
    [SerializeField] float diveForceDown = 2f;
    [SerializeField] float diveForceUp = 2f;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (isInWater)
        { SwimStart();}
    }
    void SwimStart() 
    {
        if (Input.GetButton("Jump") || Input.GetButtonDown("Jump"))
        {

            _rb.AddForce(Vector3.up * diveForceUp, ForceMode.Impulse);
            Debug.Log(Vector3.up * diveForceUp +"Up");
        }
        else if (Input.GetKey(KeyCode.C) ||Input.GetKeyDown(KeyCode.C))
        {
            _rb.AddForce(Vector3.down*diveForceDown,ForceMode.Impulse);
            Debug.Log(Vector3.down * diveForceDown+"Down");

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            isInWater = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Water")) 
        {
            isInWater = false;
        }
        
    }
}
