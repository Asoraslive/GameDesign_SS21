using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    private Transform target; //position des Targets
    public float range = 15f;
    public GameObject player; //Sucht nach diesem Objekt

    public Transform head; //zum rotieren
    public float turnSpeed = 25f;

    public Transform pointOfRay; // start des Raycasts

    public float timeToFire = 180f; //
    private float aktTime;

    public float damage = 50f;

    public GameObject explosionEffect;

    // Start is called before the first frame update
    void Start()
    {
        aktTime = timeToFire;
        InvokeRepeating("UpdateTarget", 0f, 0.5f);   
    }

    void UpdateTarget ()
    {
        float distanceToPlayer = Vector3.Distance(pointOfRay.position, player.transform.position);
        if (distanceToPlayer <= range)
        {
            target = player.transform;
        }
        else
        {
            target = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            aktTime = timeToFire;
            return;
        }
        Vector3 dir = target.position - pointOfRay.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(head.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        head.rotation = Quaternion.Euler(0f, rotation.y, 0f);

        RaycastHit hit;
        if (Physics.Raycast(pointOfRay.position, dir, out hit, range))
        {
            Debug.DrawLine(pointOfRay.position, hit.transform.position);
            Debug.Log(hit.transform.name + " anvisiert");
            if (hit.transform.name == player.transform.name)
            {
                aktTime--;
                if (aktTime == 0)
                {
                    Debug.Log(hit.transform.name + " wurde getroffen");
                    aktTime = timeToFire;

                    hit.transform.GetComponent<Health>().TakeDamage(damage);

                    //Instantiate(explosionEffect, hit.transform.position, hit.transform.rotation);
                }
            }
            else
            {
                aktTime = timeToFire; 
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
