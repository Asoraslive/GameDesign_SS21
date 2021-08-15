using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakWindow : MonoBehaviour
{
    [SerializeField] private GameObject window;
    [SerializeField] private GameObject window_broken;
    [SerializeField] private AudioSource shattersound;


    public void Break() {
        window.SetActive(false);
        window_broken.SetActive(true);

        shattersound.time = 1.1f;
        shattersound.Play();
    }
}
