using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckTrigger : MonoBehaviour
{
    public bool isPlayed = false;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
            isPlayed = true;
    }
}