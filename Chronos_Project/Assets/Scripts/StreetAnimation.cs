using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreetAnimation : MonoBehaviour {
    private Vector3 direction;
    private Quaternion rotation;
    [SerializeField]
    private GameObject car;

    [SerializeField] private Transform[] points;

    [SerializeField]
    private float speed;

    [SerializeField]
    private bool loop = false;

    private int visited_point = 0;
  
    private bool active = true;
    [SerializeField]
    private float waitseconds;
    private float starttime;

    void Start() {
        //points = GetComponentsInChildren<Transform>();
        direction = points[1].transform.position - points[0].transform.position;
        car.transform.position = points[0].transform.position;
        car.transform.eulerAngles = convert_Vector_Quaternion(direction);
        if (waitseconds != 0) {
            active = false;
            starttime = Time.fixedTime;
        }
    }


    // Update is called once per frame
    void Update() {
        if (active) {
            car.transform.position += (direction.normalized * speed) * Time.deltaTime;
            if ((car.transform.position - points[visited_point].transform.position).magnitude >= direction.magnitude) {
                if(visited_point == points.Length - 2) {
                    if (!loop) {
                        Destroy(car);
                        Destroy(this);
                    }
                    else {
                        car.transform.position = points[0].transform.position;
                        visited_point = 0;
                        direction = points[1].transform.position - points[0].transform.position;
                        car.transform.eulerAngles = convert_Vector_Quaternion(direction);
                    }
                    
                }
                else {
                    visited_point++;
                    direction = points[visited_point + 1].transform.position - points[visited_point].transform.position;
                    car.transform.eulerAngles = convert_Vector_Quaternion(direction);
                }
                
            }
        }
        else {
            if (Time.fixedTime > starttime + waitseconds) {
                active = true;
            }
        }
    }

    Vector3 convert_Vector_Quaternion(Vector3 v) {         // Result is only Top Down View, other Angles are ignored !!!
        //Vector3 x_standard = new Vector3(1,0,0);
        //Vector3 direction_x_projection = new Vector3(direction.x, 0, direction.z).normalized;
        //double x_angle = Math.Acos(Vector3.Dot(x_standard,direction_x_projection)/(x_standard.magnitude * direction_x_projection.magnitude));

        //Vector3 z_standard = new Vector3(1, 0, 0);
        //Vector3 direction_z_projection = new Vector3(direction.x, 0, direction.z).normalized;
        //double z_angle = Math.Acos(Vector3.Dot(z_standard, direction_z_projection) / (z_standard.magnitude * direction_z_projection.magnitude));

        Vector3 forward = new Vector3(1,0,0);

        Vector3 y_standard = new Vector3(0, 1, 0);
        Vector3 direction_y_projection = new Vector3(v.x, 0, v.z).normalized;
        double y_angle = Math.Acos(Vector3.Dot(forward, direction_y_projection));
        //Debug.Log("Skalar Produkt: " + Vector3.Dot(forward, direction_y_projection));
        //Debug.Log(v);
        //Debug.Log(v.normalized);
        //Debug.Log("Winkel: " + y_angle);
        float angle = (float)(y_angle * 180 / Math.PI);
        if(v.z > 0) {
            angle = -angle;
        }
        return new Vector3(0,angle,0);
    }
}
