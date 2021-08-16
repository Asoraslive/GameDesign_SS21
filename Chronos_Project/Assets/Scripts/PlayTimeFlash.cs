using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayTimeFlash : MonoBehaviour
{
    public Animator flash;
    private bool played = false;
    [SerializeField] AudioSource timeTravelSound;
    
    void Update()
    {

    }

    public void Flash()
    {
        StartCoroutine(PlayAnimationFlash());
    }

    IEnumerator PlayAnimationFlash ()
    {
        flash.SetTrigger("Flash");
        timeTravelSound.Play();
        played = true;

        yield return new WaitForSeconds(1f);
        timeTravelSound.Stop();
        played = false;
    }
}
