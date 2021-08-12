using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Vector3 dir;
    public float speed = 50f;
    public Rigidbody rb;
    public float dmg = 5;

    public void Seek(Vector3 _dir)
    {
        dir = _dir;
        
        //Debug.Log(dir);
    }

    // Update is called once per frame
    void Update()
    {
        if(dir == null)
        {
            Destroy(gameObject);
            return;
        }

        rb.velocity = dir.normalized * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        if (other.transform.name == "Player")
        {
            other.GetComponent<Health>().TakeDamage(dmg);
        }
        Destroy(gameObject);
        
    }
}
