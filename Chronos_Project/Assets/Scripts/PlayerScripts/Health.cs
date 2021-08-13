using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public int maxHealth = 100;
    public float currentHealth;
    public Transform respawn;
    private bool healthRegen = false;
    public float regenDelay = 2f;
    public float regenPerFrame = 1f;
    [SerializeField] Image fill;
    [SerializeField] GameObject hpBar;
    [SerializeField] GameObject player;


    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) TakeDamage(15f);
    }

    private void FixedUpdate()
    {
        if (healthRegen && currentHealth < maxHealth)
        {
            currentHealth += regenPerFrame;
            fill.fillAmount = Mathf.Lerp(fill.fillAmount, currentHealth / maxHealth, 3f);

            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }
        }
    }

    public void TakeDamage(float dmg)
    {
        healthRegen = false;
        currentHealth -= dmg;
        if (regenCRActive) { StopCoroutine(StartRegen()); regenCRActive = false; }
        //healthbar.updateHealth((int)currentHealth);
        if (currentHealth <= 0)
        {
            Die();
            return ;
        }
        //current health in Hud 
        LeanTween.scale(hpBar, new Vector3(1.1f, 1.1f, 1.1f), 1f).setEaseInOutBack().setLoopPingPong(1);
        fill.fillAmount = Mathf.Lerp(fill.fillAmount, currentHealth / maxHealth, 1f);

        StartCoroutine(StartRegen());
        return ;
    }

    public void Die()
    {
        player.transform.position = respawn.position;
        Rigidbody _rb = GetComponent<Rigidbody>();
        _rb.velocity = Vector3.zero;
        currentHealth = maxHealth;
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

    private bool regenCRActive = false;

    IEnumerator StartRegen()
    {
        regenCRActive = true;
        yield return new WaitForSeconds(regenDelay);
        healthRegen = true;
        regenCRActive = false;
    }

}
