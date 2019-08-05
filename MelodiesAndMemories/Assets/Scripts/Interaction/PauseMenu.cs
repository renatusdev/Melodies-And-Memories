using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu pause;

    public bool canPause;
    public int sensScale;
    public GameObject GUI_Pause;

    private CinemachinePOV pov;
    private bool inMenu;


    private void Awake()
    {
        if (pause == null)
            pause = this;
    }

    void Start()
    {
        inMenu = false;
    }

    void LateUpdate()
    {
        if (Input.GetButtonDown("Cancel") & canPause)
        {
            if (!inMenu)
                StartMenu();
            else
                LeaveMenu();
        }
    }

    void StartMenu()
    {
        pov = PlayerController.player.getPOV();
        GUI_Pause.SetActive(true);
        Cursor.visible = true;
        AudioManager.manager.PauseAllSounds();
        Time.timeScale = 0f;
        inMenu = true;
    }

    public void LeaveMenu()
    {
        GUI_Pause.SetActive(false);
        Cursor.visible = false;
        AudioManager.manager.UnPauseAllSounds();
        Time.timeScale = 1f;
        inMenu = false;
    }

    public void AddSensitivity()
    {
        pov.m_HorizontalAxis.m_MaxSpeed += sensScale;
        pov.m_VerticalAxis.m_MaxSpeed += sensScale;
    }

    public void LessSensitivity()
    {
        pov.m_HorizontalAxis.m_MaxSpeed -= sensScale;
        pov.m_VerticalAxis.m_MaxSpeed -= sensScale;
    }

    public void Exit() { Application.Quit(); }

    public void SetCanPause(bool status)
    {
        canPause = status;
    }
}
