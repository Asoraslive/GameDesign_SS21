using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumppad : MonoBehaviour
{
    [SerializeField] private float force = 20f;
    [SerializeField] private float upforce = 20f;
    [SerializeField] private bool active = false;

    [SerializeField] private Material lights_deactivated;
    [SerializeField] private Material lights_activated;
    [SerializeField] private float soundStart;
    [SerializeField] private float soundEnd;
    [SerializeField] private AudioSource jumpPadSound;

    private void OnTriggerEnter(Collider other)
    {
        if (active) {
            if (other.CompareTag("Player")) {
                other.GetComponent<Rigidbody>().AddForce(transform.up * force + other.transform.up * upforce, ForceMode.Impulse);
                StartCoroutine("playJumpPadSound");
            }
        }
        
    }

    public void activate(bool status) {
        active = status;
        if (status) {
            gameObject.GetComponent<Renderer>().materials[1] = Instantiate<Material>(lights_activated);
            lights_deactivated.Lerp(lights_deactivated, lights_activated, 3.0f);
        }
        else {
            gameObject.GetComponent<Renderer>().materials[1] = lights_deactivated;
        }
    }

    IEnumerator playJumpPadSound()
    {
        jumpPadSound.time = soundStart;
        jumpPadSound.Play();
        yield return new WaitForSeconds(soundEnd);
        jumpPadSound.Stop();
    }
}
