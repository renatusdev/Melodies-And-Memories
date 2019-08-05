using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AudioManager : MonoBehaviour
{
    [Range(0,1)] public float ambienceVol;


    public static AudioManager manager;
    public List<AudioSource> sounds;
    public int filterWaitTime;

    private AudioSource currentMusic;

    private void Awake()
    {
        if (manager == null)
            manager = this;
    }

    void Start()
    {
        filterWaitTime = 5;
        sounds = new List<AudioSource>();
        currentMusic = GetComponent<AudioSource>();
        StartCoroutine(CompletionFilter());
    }

    private IEnumerator CompletionFilter()
    {
        yield return new WaitForSeconds(filterWaitTime);

        for(int i = 0; i < sounds.Count; i++)
        {
            AudioSource source = sounds[i];
            if (source == null)
                continue;

            if (!source.isPlaying)
                RemoveSound(source);
        }

        StartCoroutine(CompletionFilter());
    }

    public void RemoveSound(AudioSource sound)
    {
        sounds.Remove(sounds.Find(sounds => sounds == sound));
        sound.Stop();
    }

    public void UseSound(AudioSource sound)
    {
        sounds.Add(sound);
        sound.Play();
    }

    public void PauseAllSounds()
    {
        currentMusic.Pause();
        foreach (AudioSource source in sounds)
            source.Pause();
    }

    public void UnPauseAllSounds()
    {
        currentMusic.UnPause();
        foreach (AudioSource source in sounds)
            source.UnPause(); 
    }

    public void FadeMusic(float volume, float duration)
    {
        currentMusic.DOFade(volume, duration);
    }

    public AudioSource getCurrentMusic()
    {
        return currentMusic;
    }

    public float GetAmbienceVol()
    {
        return ambienceVol;
    }

    public void SetAmbienceVol(float vol)
    {
        ambienceVol = vol;
    }
}