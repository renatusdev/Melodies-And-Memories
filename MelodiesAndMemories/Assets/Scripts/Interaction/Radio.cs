using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Radio : MonoBehaviour
{
    public static Radio on;

    private Image radioUI;

    void Start()
    {
        if (on == null)
            on = this;

        radioUI = GetComponent<Image>();
    }

    public IEnumerator Relay(AudioSource message)
    {
        radioUI.enabled = true;
        AudioManager.manager.UseSound(message);

        yield return new WaitForSeconds(message.clip.length);

        radioUI.enabled = false;
    }
}
