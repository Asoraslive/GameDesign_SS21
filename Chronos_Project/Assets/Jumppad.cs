using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumppad : MonoBehaviour
{
    [SerializeField] private float force = 20f;
    [SerializeField] private bool active = false;

    [SerializeField] private Material lights_deactivated;
    [SerializeField] private Material lights_activated;
    [SerializeField] private Material status_light;

    private void Start() {
        status_light = lights_deactivated;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (active) {
            if (other.CompareTag("Player")) {
                other.GetComponent<Rigidbody>().AddForce(transform.up * force);
            }
        }
        
    }

    public void activate(bool status) {
        active = status;
        if (status) {
            status_light = lights_activated;
        }
        else {
            status_light = lights_deactivated;
        }
    }
}
