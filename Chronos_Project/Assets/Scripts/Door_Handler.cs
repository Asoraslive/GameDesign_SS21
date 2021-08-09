using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door_Handler : MonoBehaviour {

    [SerializeField] private float open_width;


    //public Vector3 direction;
    [SerializeField] private float speed;
    [SerializeField] private Transform left_door;
    [SerializeField] private Transform right_door;

    private bool opening = true;

    private bool active = false;

    private bool lockdown = false;

    [SerializeField] private Material locked_material;
    [SerializeField] private Material lockdown_material;

    // Update is called once per frame
    void Update() {
        if (active) {
            if (opening) {  // Open Doors
                left_door.position += (new Vector3(1,0,0) * speed) * Time.deltaTime;
                right_door.position += (new Vector3(-1, 0, 0) * speed) * Time.deltaTime;
            }
            else {          // Close Doors
                left_door.position += (new Vector3(-1, 0, 0) * speed) * Time.deltaTime;
                right_door.position += (new Vector3(1, 0, 0) * speed) * Time.deltaTime;
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (!active && !lockdown) {
            opening = true;
            StartCoroutine(WaitCoroutine());
        }
    }

    private void OnTriggerExit(Collider other) {
        if (!active & !lockdown) {
            opening = false;
            StartCoroutine(WaitCoroutine());
        }
    }

    IEnumerator WaitCoroutine() {
        active = true;
        yield return new WaitForSeconds(open_width / speed);
        active = false;
    }

    public void lockdownStatus(bool status) {
        lockdown = status;
        if (status) {
            locked_material.Lerp(locked_material, lockdown_material, 3.0f);
        }
        else {
            locked_material.Lerp(lockdown_material, locked_material, 3.0f);
        }
    }
}
