using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class StartGame : MonoBehaviour
{
    public Light screenLight;
    public Image background;
    public AudioSource menuMusic;
    public ParticleSystemRenderer UIEffect;

    private bool pressedEnter;

    private void Start()
    {
        background.canvasRenderer.SetAlpha(0);
        pressedEnter = false;
        screenLight.DOIntensity(1, 5);
        menuMusic.DOFade(.2f, 8);
    }

    void Update()
    {
        if (Time.fixedTime >= 5 && Input.GetKeyDown(KeyCode.Return) & !pressedEnter)
            StartCoroutine(LoadGameScene());
    }

    IEnumerator LoadGameScene()
    {
        pressedEnter = true;
        GetComponent<AudioSource>().Play();
        background.CrossFadeAlpha(1, .1f, false);
        UIEffect.material.DOColor(Color.black, 1);

        yield return new WaitForSeconds(1);

        background.CrossFadeAlpha(0, .3f, false);

        screenLight.DOIntensity(0, 2);
        menuMusic.DOFade(0, 5);
        GetComponent<AudioSource>().DOFade(0, 5);

        yield return new WaitForSeconds(5);

        SceneManager.LoadScene(1);
    }
}
