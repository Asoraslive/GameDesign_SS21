using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakWindow : MonoBehaviour
{
    [SerializeField] private GameObject window;
    [SerializeField] private GameObject window_broken;
    [SerializeField] private AudioSource shattersound;

    [SerializeField] bool lockdown_active = false;


    public void Break() {
        if (lockdown_active) {
            window.SetActive(false);
            window_broken.SetActive(true);

            shattersound.time = 1.1f;
            shattersound.Play();
        }
    }

    public void LockdownStart() {
        lockdown_active = true;
    }
}
