using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenTheDoor : MonoBehaviour
{
    private bool played;

    private void Start()
    {
        played = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") & !played)
        {
            AudioManager.manager.UseSound(GetComponent<AudioSource>());
            played = true;
        }    
    }
}
