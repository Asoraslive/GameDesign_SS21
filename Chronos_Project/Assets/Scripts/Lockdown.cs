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
    [SerializeField] Tool_Tips tooltips;


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
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other) {
        Debug.LogWarning("Enter E Trigger");
        activate_e = true;
        tooltips.Tip_e(true);
    }

    private void OnTriggerExit(Collider other) {
        Debug.LogWarning("Exit E Trigger");
        activate_e = false;
        tooltips.Tip_e(false);
    }

    private void activateLockDown() {
        Debug.Log("Activte LockDown Protocol !");
        pickUpSound.Play();
       
    }
}
