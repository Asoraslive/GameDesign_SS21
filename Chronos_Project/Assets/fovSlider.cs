using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class fovSlider : MonoBehaviour
{

    // Start is called before the first frame update
    void OnEnable()
    {
        PlayerController pC = FindObjectOfType<PlayerController>();
        Slider slider = GetComponent<Slider>();
        slider.value = pC.getFov() / 100f;
    }

 
}
