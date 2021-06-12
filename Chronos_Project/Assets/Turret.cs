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

    public float timeToFire = 180f;
    private float aktTime;

    public float damage = 50f;

    public GameObject explosionEffect;

    public LineRenderer lr;

    private Vector3 nullV = new Vector3(0,0,0);

    public AudioSource aktivate;
    private bool soundplayed = false;

    // Start is called before the first frame update
    void Start()
    {
        aktTime = timeToFire;
        InvokeRepeating("UpdateTarget", 0f, 0.5f); //sucht nach dem Spieler jede 0.5 Sekunde
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
            soundplayed = false;
            lr.SetPosition(1, nullV);
            aktTime = timeToFire;
            return;
        }

        //Rotiert den Kopf
        Vector3 dir = target.position - pointOfRay.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(head.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        head.rotation = Quaternion.Euler(0f, rotation.y, 0f);

        //Wenn der Spieler anvisiert ist, wird der Timer runtergezählt. Wenn der Spieler nicht mehr anvisiert ist, resettet der Timer. Wenn der Timer abgelaufen ist feuert der Turm
        RaycastHit hit; 
        if (Physics.Raycast(pointOfRay.position, dir, out hit, range))
        {
            if (hit.transform.name == player.transform.name)
            {
                aktivate.Play();
                soundplayed = true;
                lr.SetPosition(1, hit.transform.position);
                aktTime--;
                if (aktTime == 0)
                {
                    aktTime = timeToFire;

                    hit.transform.GetComponent<Health>().TakeDamage(damage);

                    Instantiate(explosionEffect, hit.transform.position, hit.transform.rotation);
                }
            }
            else
            {
                lr.SetPosition(1, nullV);
                aktTime = timeToFire; 
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
