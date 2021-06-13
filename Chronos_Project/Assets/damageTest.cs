using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class damageTest : MonoBehaviour
{
    public Healthbar healthbar;
    int maxHealth = 100;
    int currentHealth;
    int dmg = 20;
    private void Start()
    {
        currentHealth = maxHealth;
        healthbar.SetMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump")){
            currentHealth -= dmg;
            healthbar.updateHealth(currentHealth);
        }
    }
}
