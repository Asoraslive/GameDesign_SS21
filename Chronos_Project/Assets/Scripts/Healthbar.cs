using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    

    [Header("References")]
    [SerializeField] GameObject red;
    [SerializeField] GameObject green;
    [SerializeField] Slider slider;

    //Starter function
    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        updateHealth(health);
    }

    //Updating health after Dmg or Heal
    public void updateHealth(int health)
    {
        slider.value = health;
        changeHealthColour();
    }

    /*Swap between Health Color at 33% 
     */
    public void changeHealthColour()
    {
        if (slider.value > (slider.maxValue * .33f))
        {
            red.SetActive(false);
            green.SetActive(true);
        }
        else
        {
            red.SetActive(true);
            green.SetActive(false);
        }
    } 

}
