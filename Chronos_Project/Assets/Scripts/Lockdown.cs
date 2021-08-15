using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Lockdown : MonoBehaviour{

    private bool activate_e = false;

    [SerializeField] private Material animated_light_bar;
    [SerializeField] private Material light_bar_lockdown;
    [SerializeField] private AudioSource pickUpSound;
    [SerializeField] private AudioSource lockDownSound;


    [SerializeField]
    private UnityEvent startLockDown;

    // Start is called before the first frame update
    void Start()
    {
        animated_light_bar.Lerp(animated_light_bar, animated_light_bar, 3.0f);
    }

    // Update is called once per frame
    void Update(){
        if (activate_e && Input.GetKey(KeyCode.E)) {
            activateLockDown();
            startLockDown.Invoke();
        }
    }

    private void OnTriggerEnter(Collider other) {
        activate_e = true;
    }

    private void OnTriggerExit(Collider other) {
        activate_e = false;
    }

    private void activateLockDown() {
        Debug.Log("Activte LockDown Protocol !");
        pickUpSound.Play();
        //animated_light_bar.color = Color.red;
        StartCoroutine("ldSoundPlayer");
    }

    IEnumerator ldSoundPlayer()
    {
        yield return new WaitForSeconds(1f);
        animated_light_bar.Lerp(animated_light_bar, light_bar_lockdown, 3.0f);
        lockDownSound.Play();
    }
}
