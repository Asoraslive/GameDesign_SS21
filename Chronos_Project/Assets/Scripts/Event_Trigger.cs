using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Event_Trigger : MonoBehaviour {

    private bool activate_e = false;
    private bool activated = false;

    [SerializeField]
    private UnityEvent TriggerEvent;
    [SerializeField] private AudioSource buttonSound;

    // Start is called before the first frame update

    // Update is called once per frame
    void Update() {
        if (!activated && activate_e && Input.GetKey(KeyCode.E)) {
            TriggerEvent.Invoke();
            buttonSound.Play();
            activated = true;
        }
    }

    private void OnTriggerEnter(Collider other) {
        activate_e = true;
    }

    private void OnTriggerExit(Collider other) {
        activate_e = false;
    }
}
