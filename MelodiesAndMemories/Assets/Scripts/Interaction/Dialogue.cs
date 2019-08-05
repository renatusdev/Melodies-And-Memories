using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (AudioSource))]
public class Dialogue : MonoBehaviour
{
    public string[] monologue;
    public AudioClip[] clips;

    private CheckTrigger[] triggers;
    private int clipCount;
    private bool notPlayed = true;
    private AudioSource dialogue;

    private void Start()
    {
        triggers = GetComponentsInChildren<CheckTrigger>();
        dialogue = GetComponent<AudioSource>();
        clipCount = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            dialogue.clip = clips[clipCount];

            if (monologue.Length.Equals(0))
            {
                StartCoroutine(Radio.on.Relay(dialogue));
            }
            else
            {
                StartCoroutine(Radio.on.Relay(dialogue));
                StartCoroutine(Converse.say.Text(monologue[clipCount], dialogue.clip.length));
            }

            clipCount++;

            foreach (CheckTrigger t in triggers)
                if (t.isPlayed)
                    t.gameObject.SetActive(false);
        }
    }
}
