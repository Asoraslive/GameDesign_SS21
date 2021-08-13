using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{

    [Header("References")]
    public Transform target; //position des Targets
    public GameObject player; //Sucht nach diesem Objekt
    public Transform head; //zum rotieren
    public Transform pointOfRay; // start des Raycasts
    public GameObject projectilePrefab;

    public Transform body;
    public Transform cannon;

    //public LineRenderer lr;
    //public GameObject explosionEffect;
    //public AudioSource aktivate;
    //public AudioClip explosionsfx;

    [Header("Turret Specs")]
    public float turnSpeed = 25f;
    public float range = 15f;
    public float timeToFire = 180f;
    public bool rndTimeToFire = false;
    public float minRandomTTF = 80f;
    public float maxRandomTTF = 150f;
    public float delayBetweenShots = 3f;
   

    [Header("Explosion")]
    //public float explosionradius = 1f;
    //public float explosionForce = 20f;

    //Privates
    //private bool soundplayed = false;
    private float aktTime;
    private float delayTime;


    // Start is called before the first frame update
    void Start()
    {
        if (rndTimeToFire)
        {
            aktTime = Random.Range(80f, 150f);
        }
        else
        {
            aktTime = timeToFire;
            delayTime = delayBetweenShots;
        }

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
    void Update()
    {
        if (target == null)
        {
            aktTime = timeToFire;
        }

            //Rotiert den Kopf
            Vector3 dir = getTarget();
            Quaternion lookRotation = Quaternion.LookRotation(dir);
            Vector3 rotation = Quaternion.Lerp(head.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
            head.rotation = Quaternion.Euler(0f, rotation.y, 0f);
            Debug.DrawRay(pointOfRay.position, dir, Color.cyan);
        
            if(aktTime > 0)
            {
                aktTime--;
            }
            else if (aktTime <= 0)
            {
                if (delayTime <= 0)
                {
                    delayTime = delayBetweenShots;
                    GameObject projectileGO = (GameObject)Instantiate(projectilePrefab, pointOfRay.position, pointOfRay.rotation);
                    Projectile projectile = projectileGO.GetComponent<Projectile>();
                }
                else
                {
                    delayTime--;
                } 
            }
    }

    private Vector3 getTarget()
    {
        return target.position - pointOfRay.position;
    }

    /*IEnumerator Shoot (Vector3 direction)
    {
        yield return new WaitForSeconds(0.05f);
        RaycastHit t;
        
        if(Physics.Raycast(pointOfRay.position, direction, out t, range))
        {
            AudioSource.PlayClipAtPoint(explosionsfx, t.point);
            GameObject explo = Instantiate(explosionEffect, t.point, t.transform.rotation);
            ParticleSystem pSys = explo.GetComponent<ParticleSystem>();
            float totalDuration = pSys.duration + pSys.startLifetime;
            Destroy(explo, totalDuration);

            Collider[] hitCollider = Physics.OverlapSphere(t.transform.position, explosionradius);
            foreach (Collider c in hitCollider)
            {
                if (c.CompareTag("Player"))
                {
                    
                    Rigidbody _rb = c.GetComponent<Rigidbody>();
                    Vector3 pushForce = c.transform.position - t.point;
                    Health h = c.GetComponent<Health>();

                    if (!h.TakeDamage())
                    {
                        _rb.AddForce(pushForce.normalized * explosionForce, ForceMode.Impulse);
                    }
                }

            }
        }
        shot = false;
    }*/


    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
