using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayTimeFlash : MonoBehaviour
{
    public Animator flash;
    private bool played = false;
    
    void Update()
    {
        if (Input.GetMouseButtonDown(1) && !played)
        {
            StartCoroutine(PlayAnimationFlash());
        }
    }

    IEnumerator PlayAnimationFlash ()
    {
        flash.SetTrigger("Flash");
        played = true;

        yield return new WaitForSeconds(0.5f);
        played = false;
    }
}
