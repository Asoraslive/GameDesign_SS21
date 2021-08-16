using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raum_Lockdown : MonoBehaviour
{
    [SerializeField] private Material animated_light_bar;
    [SerializeField] private Material light_bar_lockdown;
    [SerializeField] private AudioSource lockDownSound;

    public void startLockdown() {
        StartCoroutine("ldSoundPlayer");
    }

    IEnumerator ldSoundPlayer() {
        yield return new WaitForSeconds(1f);
        animated_light_bar.Lerp(animated_light_bar, light_bar_lockdown, 3.0f);
        lockDownSound.Play();
    }
}
