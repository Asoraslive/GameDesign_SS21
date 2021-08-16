using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Event_Trigger : MonoBehaviour {

    private bool activate_e = false;
    private bool activated = false;
    private bool activated_tooltip = false;

    [SerializeField]
    private UnityEvent TriggerEvent;
    [SerializeField] private AudioSource buttonSound;
    [SerializeField] bool useButtonSound = true;
    [SerializeField] Tool_Tips tooltips;
    [SerializeField] int tooltip_action = 1;
    [SerializeField] bool needLockdown = false;
    private bool lockdownactive = false;

    // Start is called before the first frame update

    // Update is called once per frame
    void Update() {
        if (!activated && activate_e && Input.GetKey(KeyCode.E)) {
            TriggerEvent.Invoke();
            if (useButtonSound) {
                buttonSound.Play();
            }
            activated = true;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (!activated_tooltip && needLockdown && lockdownactive) {
            if(tooltip_action == 1) {
                tooltips.Tip_e(true);
            }
            else {
                tooltips.Tip_timeswap(true);
            }
           
            activated_tooltip = true;
        }
        if (needLockdown) {
            if (lockdownactive) {
                activate_e = true;
            }
        }
        else {
            activate_e = true;
        }
       
    }

    private void OnTriggerExit(Collider other) {
        activate_e = false;
        if(tooltip_action == 1) {
            tooltips.Tip_e(false);
        }
        else
        {
            tooltips.Tip_timeswap(false);
        }
       
    }

    public void startLockdown() {
        lockdownactive = true;
    }
}
