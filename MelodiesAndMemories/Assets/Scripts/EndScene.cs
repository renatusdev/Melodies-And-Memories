using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndScene : MonoBehaviour
{
    public static EndScene epilogue;
    public Transform teleport;
    public AudioSource radio, ambience;
    public Image background;

    private Animator myAnimator;

    private void Awake()
    {
        if (epilogue == null)
            epilogue = this;
    }

    private void Start()
    {
        myAnimator = GetComponent<Animator>();
    }

    public IEnumerator End()
    {

        yield return new WaitForSeconds(2);

        ScreenFlash.flash.Play();
        PlayerController.player.SetCanMove(false);

        yield return new WaitForSeconds(1);

        Destroy(transform.GetChild(0).gameObject);
        PlayerController.player.transform.SetParent(transform);
        PlayerController.player.transform.position = teleport.position;

        yield return new WaitForSeconds(9);

        myAnimator.enabled = true;
        myAnimator.Rebind();

        ScreenFlash.flash.Stop();

        yield return new WaitForSeconds(2);

        myAnimator.SetBool("startEnd", true);

    }

    void FirstRadio()
    {
        radio.Play();
    }

    void BackgroundEffect()
    {
        background.enabled = true;
    }

    void EndAmbience()
    {
        ambience.Play();
    }

    void Bye()
    {
        Debug.Log("it quit");
        Application.Quit();
    }

}