using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MusicBoxInteraction : MonoBehaviour
{
    [Range(0, 1)] public float memoryVol;
    [Range(0, 1)] public float melodyVol;

    public bool finishMelody;
    public AudioSource memory;

    [SerializeField]
    private bool pickedUp;
    private Animator myAnimator;
    private AudioSource melody;
    private float droppedVolume;

    private void Start()
    {
        myAnimator = GetComponent<Animator>();
        melody = GetComponent<AudioSource>();

        pickedUp = false;
        droppedVolume = 0.5f;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            myAnimator.SetBool("isInteracting", true);
            AudioManager.manager.UseSound(melody);
            AudioManager.manager.UseSound(memory);
            AudioManager.manager.FadeMusic(droppedVolume, 2f);
            melody.DOFade(melodyVol, 2);
            memory.DOFade(memoryVol, 2);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && !pickedUp)
        {
            melody.DOFade(0, .5f).OnComplete(() => AudioManager.manager.RemoveSound(melody));
            memory.DOFade(0, .5f).OnComplete(() => AudioManager.manager.RemoveSound(memory));
            AudioManager.manager.FadeMusic(AudioManager.manager.GetAmbienceVol(), .5f);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
            Interaction();
    }

    void Interaction()
    {
        if (Input.GetButtonDown("Interact") & PlayerController.player.Distance(gameObject) < 4 & !pickedUp)
            Store();
    }

    void Store()
    {
        pickedUp = true;
        GameObject.FindGameObjectWithTag("Inventory").GetComponent<InventoryController>().AddMusicBox();
        myAnimator.SetBool("isInteracting", false);

        if (finishMelody)
            StartCoroutine(FinishMelody());
        else
        {
            AudioManager.manager.FadeMusic(AudioManager.manager.GetAmbienceVol(), .5f);
            Destroy(gameObject);
        }
    }

    IEnumerator FinishMelody()
    {
        yield return new WaitUntil(() => !melody.isPlaying);
        AudioManager.manager.FadeMusic(AudioManager.manager.GetAmbienceVol(), .5f);
        myAnimator.enabled = false;
        Destroy(gameObject);
    }
}
