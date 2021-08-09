using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public Healthbar healthbar;
    int maxHealth = 100;
    public float currentHealth;
    int damage = 34;
    public Transform respawn;
    private bool healthRegen;
    public float regenDelay = 2f;
    public float regenPerFrame = 1f;


    private void Start()
    {
        currentHealth = maxHealth;
        healthbar.SetMaxHealth(maxHealth);
        healthRegen = true;
    }

    private void FixedUpdate()
    {
        if (healthRegen && currentHealth <= maxHealth)
        {
            currentHealth += regenPerFrame;
            if(currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }
        }
    }

    public bool TakeDamage(float dmg)
    {
        currentHealth -= dmg;
        StopCoroutine("StartRegen");
        healthbar.updateHealth((int)currentHealth);
        if (currentHealth <= 0)
        {
            Die();
            return true;
        }
        healthRegen = false;
        StartCoroutine("StartRegen");
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

    IEnumerator StartRegen()
    {
        yield return new WaitForSeconds(regenDelay);
        healthRegen = true;
    }
}
