using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Event_Trigger : MonoBehaviour {

    private bool activate_e = false;

    [SerializeField]
    private UnityEvent TriggerEvent;

    // Start is called before the first frame update

    // Update is called once per frame
    void Update() {
        if (activate_e && Input.GetKey(KeyCode.E)) {
            TriggerEvent.Invoke();
        }
    }

    private void OnTriggerEnter(Collider other) {
        activate_e = true;
    }

    private void OnTriggerExit(Collider other) {
        activate_e = false;
    }
}
