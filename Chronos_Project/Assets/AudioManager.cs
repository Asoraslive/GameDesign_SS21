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
        source.volume = .5f;
        source.PlayOneShot(clips[0]);
    }

    private void runAStep()
    {
        source.volume = .8f;
        source.PlayOneShot(clips[1]);
    } 
    private void jumpUp()
    {
        source.volume = 1f;
        source.PlayOneShot(clips[2]);
    }

}
