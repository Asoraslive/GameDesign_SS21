using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [Header("References")]
    [SerializeField] WallRun wallRun;

    [SerializeField] private float sensX = 100f;
    [SerializeField] private float sensY = 100f;
    [SerializeField] float rotationSpeedCharacter;


    [SerializeField] Transform cam;
    [SerializeField] Transform orientation;
    [SerializeField] Transform charmodel;


    float mouseX;
    float mouseY;

    float multiplier = 0.01f;

    float xRotation;
    float yRotation;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void Update()
    {
        if(!PauseButton.GameIsPaused)
        {
            mouseX = Input.GetAxisRaw("Mouse X");
            mouseY = Input.GetAxisRaw("Mouse Y");

            yRotation += mouseX * sensX * multiplier;
            xRotation -= mouseY * sensY * multiplier;

            //Clamp
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            cam.transform.localRotation = Quaternion.Euler(xRotation, yRotation, wallRun.tilt);
            orientation.transform.rotation = Quaternion.Euler(0, yRotation, 0);


            charmodel.transform.rotation = Quaternion.RotateTowards(charmodel.transform.rotation, orientation.transform.rotation, rotationSpeedCharacter * Time.deltaTime);
        }
    }

}
