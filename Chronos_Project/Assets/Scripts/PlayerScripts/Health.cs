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

    public bool TakeDamage()
    {
        currentHealth -= dmg;
        healthbar.updateHealth(currentHealth);
        if (currentHealth <= 0)
        {
            Die();
            return true;
        }

        return false;
    }

    public void Die()
    {
        transform.position = respawn.position;
        Rigidbody _rb = GetComponent<Rigidbody>();
        _rb.velocity = Vector3.zero;
        currentHealth = maxHealth;
        healthbar.updateHealth(maxHealth);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other);
        if (other.CompareTag("Respawn"))
        {
            respawn = other.transform;
        }
        if (other.CompareTag("Deathbox"))
        {
            Die();
        }
    }
}
