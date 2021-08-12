using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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

        transform.eulerAngles = convert_Vector_Quaternion(dir);
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

    Vector3 convert_Vector_Quaternion(Vector3 v)
    {
        Vector3 x_standard = new Vector3(0, 1, 0);
        Vector3 direction_x_projection = new Vector3(0, v.y, v.z).normalized;
        double x_angle = Math.Acos(Vector3.Dot(x_standard, direction_x_projection));
        float angle_x = (float)(x_angle * 180 / Math.PI);

        Vector3 z_standard = new Vector3(0, 1, 0);
        Vector3 direction_z_projection = new Vector3(0, v.y, v.z).normalized;
        double z_angle = Math.Acos(Vector3.Dot(z_standard, direction_z_projection));
        float angle_z = (float)(z_angle * 180 / Math.PI);

        Vector3 y_standard = new Vector3(1, 0, 0);
        Vector3 direction_y_projection = new Vector3(v.x, 0, v.z).normalized;
        double y_angle = Math.Acos(Vector3.Dot(y_standard, direction_y_projection));
        float angle_y = (float)(y_angle * 180 / Math.PI);

        if (v.z > 0)
        {
            angle_y = -angle_y;
        }
        if (v.z < 0)
        {
            angle_x = -angle_x;
        }
        if (v.x < 0)
        {
            angle_z = -angle_z;
        }
        return new Vector3(angle_x, angle_y, angle_z);
    }
}
