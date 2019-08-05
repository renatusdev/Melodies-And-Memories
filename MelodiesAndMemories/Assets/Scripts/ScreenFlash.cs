using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenFlash : MonoBehaviour
{
    public static ScreenFlash flash;

    private void Awake()
    {
        if (flash == null)
            flash = this;
    }

    public void Play()
    {
        PauseMenu.pause.SetCanPause(false);
        transform.GetChild(0).gameObject.SetActive(true);
    }

    public void Stop()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }

}
