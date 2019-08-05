using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{
    public Image GUI_Note;
    public Text GUI_Collection;
    
    [SerializeField]
    private int musicBoxes;

    private void Start()
    {
        musicBoxes = 0;
    }

    public void AddMusicBox()
    {
        musicBoxes++;
        StartCoroutine(ShowInventory());

        if (musicBoxes.Equals(5))
            StartCoroutine(EndScene.epilogue.End());
    }

    IEnumerator ShowInventory()
    {
        ActivateGUI(true);

        GUI_Collection.text = musicBoxes + " / 5";
        GUI_Note.CrossFadeAlpha(1, 5, false);
        GUI_Collection.CrossFadeAlpha(1, 5, false);

        yield return new WaitForSeconds(5);

        ActivateGUI(false);   
    }

    void ActivateGUI(bool value)
    {
        GUI_Note.gameObject.SetActive(value);
        GUI_Collection.gameObject.SetActive(value);

        GUI_Note.canvasRenderer.SetAlpha(0);
        GUI_Collection.canvasRenderer.SetAlpha(0);
    }
}
