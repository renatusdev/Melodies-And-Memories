using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Converse : MonoBehaviour
{
    public static Converse say;
    public int timeLenght;

    private Text text;

    void Start()
    {
        if (say == null)
            say = this;

        text = GetComponent<Text>();
        text.canvasRenderer.SetAlpha(0);
    }

    public IEnumerator Text (string sentece)
    {
        text.enabled = true;
        text.CrossFadeAlpha(.6f, timeLenght, false);
        text.text = sentece;

        yield return new WaitForSeconds(timeLenght);

        text.CrossFadeAlpha(0, timeLenght / 3, false);

        yield return new WaitForSeconds(timeLenght / 3);

        text.text = "";
        text.enabled = false;
    }

    public IEnumerator Text(float timeLenght, string sentence)
    {
        text.enabled = true;
        text.CrossFadeAlpha(.6f, timeLenght, false);
        text.text = sentence;

        yield return new WaitForSeconds(timeLenght);

        text.CrossFadeAlpha(0, timeLenght / 3, false);

        yield return new WaitForSeconds(timeLenght / 3);

        text.text = "";
        text.enabled = false;
    }

    public IEnumerator Text(string sentence, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        text.enabled = true;
        text.CrossFadeAlpha(.6f, timeLenght, false);
        text.text = sentence;

        yield return new WaitForSeconds(timeLenght);

        text.CrossFadeAlpha(0, timeLenght / 3, false);

        yield return new WaitForSeconds(timeLenght / 3);

        text.text = "";
        text.enabled = false;
    }
}
