using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsAtRandom : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] audioClips;

    public void PlaySounds()
    {
        audioSource.clip = audioClips[Random.Range(0, audioClips.Length)];
        AudioManager.manager.UseSound(audioSource);
    }
}