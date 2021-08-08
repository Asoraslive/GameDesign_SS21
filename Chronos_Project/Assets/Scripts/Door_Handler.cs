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

    private Vector3 start_position;

    private void Start() {
        start_position = left_door.position;
    }

    // Update is called once per frame
    void Update() {
        if (active) {
            if (opening) {  // Open Doors
                left_door.position += (new Vector3(1,0,0) * speed) * Time.deltaTime;
                right_door.position += (new Vector3(-1, 0, 0) * speed) * Time.deltaTime;
                //if ((left_door.position - start_position).magnitude >= open_width) {
                //    active = false;
                //}
            }
            else {          // Close Doors
                left_door.position += (new Vector3(-1, 0, 0) * speed) * Time.deltaTime;
                right_door.position += (new Vector3(1, 0, 0) * speed) * Time.deltaTime;
                //if ((left_door.position - start_position).magnitude <= 0.05f) {
                //    active = false;
                //}
            }
            /*if(counter == 0) {
                active = false;
            }
            counter--;*/
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (!active) {
            opening = true;
            StartCoroutine(WaitCoroutine());
        }
    }

    private void OnTriggerExit(Collider other) {
        if (!active) {
            opening = false;
            StartCoroutine(WaitCoroutine());
        }
    }

    IEnumerator WaitCoroutine() {
        active = true;
        yield return new WaitForSeconds(open_width / speed);
        active = false;
    }
}
