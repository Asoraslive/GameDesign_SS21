using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] AudioSource source;
    [SerializeField] AudioClip[] clips;

    private void takeAStep()
    {
        source.volume = .1f;
        source.PlayOneShot(clips[0]);
    }

    private void runAStep()
    {
        source.volume = .15f;
        source.PlayOneShot(clips[1]);
    } 
    private void jumpUp()
    {
        source.volume = .3f;
        source.PlayOneShot(clips[2]);
    }

}
