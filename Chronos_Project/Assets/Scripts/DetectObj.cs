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
        if(other.name != "Map")
        Obstructed = true;
    }

    private void OnTriggerStay(Collider col)
    {
        if(col.name != "Map")
        {
        Obstructed = true;
        colnow = col;
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if(col.name != "Map")
        {
        Obstructed = false;
        colnow = null;
        }
    }
}
