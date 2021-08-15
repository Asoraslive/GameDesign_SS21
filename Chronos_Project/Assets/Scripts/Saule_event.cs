using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saule_event : MonoBehaviour
{

    [SerializeField ]private bool active = false;
    [SerializeField] private Animator säulenAnim;
    [SerializeField] private AudioSource explosion;

    // Update is called once per frame
    void Update(){
        //Quaternion rotation = Quaternion.Euler(-88, 0, 0);
        if (active) {
            //transform.rotation = Quaternion.Lerp(transform.rotation, transform.rotation *= Quaternion.Euler(-88, 0, 0), 5f);
        }
    }

    public void startEvent() {
        active = true;
        //transform.rotation = Quaternion.Lerp(transform.rotation, transform.rotation *= Quaternion.Euler(0, -88, 0), 5000f);
        StartCoroutine("explodePillar");
    }

    IEnumerator explodePillar()
    {
        yield return new WaitForSeconds(1f);
        säulenAnim.SetTrigger("flipPillar");
        explosion.Play();
    }
}
