using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Projectile : MonoBehaviour
{
    public float speed = 50f;
    //public Rigidbody rb;
    public float dmg = 5;

    private float starttime;
    private int livespan = 1000;

    private void Start() {
        starttime = Time.fixedTime;
        this.GetComponent<Rigidbody>().velocity = this.transform.forward * speed * Time.deltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Time.fixedTime >= starttime + livespan) {
            Destroy(gameObject);
            Destroy(this);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.transform.name);
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Health>().TakeDamage(dmg);
        }
        Destroy(gameObject);
        
    }
}
