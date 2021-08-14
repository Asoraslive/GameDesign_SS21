using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakWindow : MonoBehaviour
{
    [SerializeField] private GameObject window;
    [SerializeField] private GameObject window_broken;

    public void Break() {
        window.SetActive(false);
        window_broken.SetActive(true);
    }
}
