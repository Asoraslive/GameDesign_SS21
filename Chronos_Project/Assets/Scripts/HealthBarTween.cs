using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarTween : MonoBehaviour
{
    [SerializeField] Image fill;
    [SerializeField] float maxHP = 100f;
    [SerializeField] float currentHp;
    [SerializeField] float dmg = 25f;
    [SerializeField] Color lowLife;
    [SerializeField] Color Life;
    [SerializeField] float hitCd=3f;
    [SerializeField] bool hitCdActive = false;
    [SerializeField] bool regenLife = false;

    [SerializeField] Health healthScript;
    // Start is called before the first frame update
    void Start()
    {
        maxHP = healthScript.maxHealth;
        currentHp = maxHP;
        fill.color = Life;
    }

    // Update is called once per frame
    void Update()
    {
        if (regenLife && currentHp < maxHP) lifeRegen();
        else if (regenLife && currentHp >= maxHP) { regenLife = false; currentHp = maxHP; }
        if (Input.GetKeyDown(KeyCode.R))
        {
            hit(dmg);
        }
    }


    public void hit(float damge)
    {
        //if hit restart Coroutine
        if (hitCdActive) StopCoroutine(hitCdRoutine());
        regenLife = false;
        currentHp = currentHp- damge;
        LeanTween.scale(gameObject, new Vector3(1.1f, 1.1f, 1.1f), 1f).setEaseInOutBack().setLoopPingPong(1);
        fill.fillAmount = Mathf.Lerp(fill.fillAmount, currentHp / maxHP, 3f);
        StartCoroutine(hitCdRoutine());
        Debug.Log(fill.fillAmount);
    }

    private IEnumerator hitCdRoutine()
    {
        hitCdActive = true;
        yield return new WaitForSeconds(hitCd);
        hitCdActive = false;
        regenLife = true;
    }
    public void lifeRegen()
    {

        currentHp += maxHP / 10 * Time.deltaTime;
        fill.fillAmount = Mathf.Lerp(fill.fillAmount, currentHp / maxHP, 3f);
    }

    public void SetMaxHealth(int max)
    {
        maxHP = max;
    }
}
