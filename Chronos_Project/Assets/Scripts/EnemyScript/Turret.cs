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
    public GameObject explosionEffect;

    public LineRenderer lr;

    private Vector3 nullV = new Vector3(0,0,0);

    public AudioSource aktivate;
    private bool soundplayed = false;

    public float explosionradius = 1f;

    private bool shot;


    // Start is called before the first frame update
    void Start()
    {
        shot = false;
        aktTime = timeToFire;
        InvokeRepeating("UpdateTarget", 0f, 0.5f); //sucht nach dem Spieler jede 0.5 Sekunden
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
    void FixedUpdate()
    {
        if (target == null)
        {
            soundplayed = false;
            lr.SetPosition(1, nullV);//schaltet den laser aus
            aktTime = timeToFire;
        }

        else if(!shot)
        {
            lr.SetColors(Color.green, Color.green);
            //Rotiert den Kopf
            Vector3 dir = getTarget();
            Quaternion lookRotation = Quaternion.LookRotation(dir);
            Vector3 rotation = Quaternion.Lerp(head.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
            head.rotation = Quaternion.Euler(0f, rotation.y, 0f);
            Debug.DrawRay(pointOfRay.position, dir, Color.cyan);
            //Wenn der Spieler anvisiert ist, wird der Timer runtergez�hlt. Wenn der Spieler nicht mehr anvisiert ist, resettet der Timer. Wenn der Timer abgelaufen ist feuert der Turm
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
                        shot = true;
                        
                        StartCoroutine(Shoot(getTarget()));
                        lr.SetColors(Color.red, Color.red);
                    }
                }
                else
                {
                    lr.SetPosition(1, nullV);
                    aktTime = timeToFire;
                }
            }


          
        }
    }

    private Vector3 getTarget()
    {
        return target.position - pointOfRay.position;
    }

    IEnumerator Shoot (Vector3 direction)
    {
        yield return new WaitForSeconds(0.05f);
        RaycastHit t;
        
        if(Physics.Raycast(pointOfRay.position, direction, out t, range))
        {
            Instantiate(explosionEffect, t.point, t.transform.rotation);
            Collider[] hitCollider = Physics.OverlapSphere(t.transform.position, explosionradius);
            foreach (Collider c in hitCollider)
            {
                if (c.CompareTag("Player"))
                {
                    c.GetComponent<Health>().TakeDamage();
                }

            }
        }
        shot = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
