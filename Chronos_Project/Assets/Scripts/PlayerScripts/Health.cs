using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public Healthbar healthbar;
    int maxHealth = 100;
    int currentHealth;
    int dmg = 34;
    public Transform respawn;



    private void Start()
    {
        currentHealth = maxHealth;
        healthbar.SetMaxHealth(maxHealth);
    }

    public void TakeDamage()
    {
        currentHealth -= dmg;
        healthbar.updateHealth(currentHealth);
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        transform.position = respawn.position;
        currentHealth = maxHealth;
        healthbar.updateHealth(maxHealth);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Respawn"))
        {
            respawn = collision.collider.transform;
        }
    }
}
