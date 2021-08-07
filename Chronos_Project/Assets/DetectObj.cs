using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectObj : MonoBehaviour
{
    public Vector3 normalOfColliding;
    public Collider colnow;
    public bool Obstructed;
    private void Awake()
    {
        colnow = GetComponent<Collider>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        normalOfColliding = collision.contacts[0].normal;
    }

    private void OnCollisionStay(Collision collision)
    {
        normalOfColliding = collision.contacts[collision.contacts.Length - 1].normal;
    }

    private void OnTriggerEnter(Collider other)
    {
        Obstructed = true;
    }

    private void OnTriggerStay(Collider col)
    {
        Obstructed = true;
        colnow = col;
    }

    private void OnTriggerExit(Collider col)
    {
        Obstructed = false;
        colnow = null;
    }
}
