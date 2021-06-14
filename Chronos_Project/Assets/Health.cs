using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    private float hp = 100f;
    public Transform respawn;

    public void TakeDamage(float amount)
    {
        hp -= amount;
        if (hp <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        transform.position = respawn.position;
    }
}
