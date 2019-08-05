using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Cinemachine;

public class GameIntro : MonoBehaviour
{
    public Light introLight;
    public Light camLight;
    public AudioSource introAmbience, camOnSound;
    public Image GUI_Center;
    public Image GUI_Talk;
    public Image GUI_Screen;
    public CinemachineVirtualCamera vCam;
    public Rigidbody playerRigidBody;
    public float introWaitTime, ambienceSound;

    void Start()
    {
        StartCoroutine(StartIntro());
    }

    IEnumerator StartIntro()
    {

        introAmbience.DOFade(AudioManager.manager.GetAmbienceVol(), 8);
        PlayerController.player.SetCanMove(false);
        GUI_Center.canvasRenderer.SetAlpha(0);
        GUI_Center.CrossFadeAlpha(1, 4, false);

        yield return new WaitForSeconds(introWaitTime);

        AudioManager.manager.UseSound(camOnSound);

        yield return new WaitForSeconds(.83f);
            
        introLight.DOIntensity(1.5f, .2f);
        camLight.DOIntensity(2.5f, .2f);
        GUI_Screen.gameObject.SetActive(true);

        yield return new WaitForSeconds(1);

        DOTween.To(() => vCam.m_Lens.FieldOfView, x => vCam.m_Lens.FieldOfView = x, 40, 2.1f);
        PlayerController.player.SetCanMove(true);
    }
}
