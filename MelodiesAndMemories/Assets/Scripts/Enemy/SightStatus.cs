using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SightStatus : MonoBehaviour
{
    EnemyBase baseData;

    void Start() {
        baseData = GetComponentInParent<EnemyBase>();
    }

    void OnTriggerEnter(Collider o)
    {
        if (o.CompareTag("Player"))
            baseData.playerInRange = true;
    }

    void OnTriggerExit(Collider o)
    {
        if (o.CompareTag("Player"))
            baseData.playerInRange = false;
    }
}