using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Cinemachine;

public class GameController : MonoBehaviour
{
    public GameObject GUI_Bar;
    public Image GUI_Seen;
    public Light screenLight;
    public CinemachineVirtualCamera cam;
    public Rigidbody playerRigidbody;

    public int stealthPoints, distanceStealthPoints;
    public int hideTimer;
    public bool hidden, gameOver;

    [SerializeField]
    private List<Transform> nearEnemyList;

    [SerializeField]
    private int hideCounter;

    private void Start()
    {
        nearEnemyList = new List<Transform>();
    }

    private void Update()
    { 
        if (!hidden)
        {
            if (hideCounter >= hideTimer)
                hidden = true;
        }

        if (stealthPoints <= 5 && !gameOver)
        {
            StartCoroutine(GameOver());
        }
    }

    IEnumerator GameOver()
    {
        gameOver = true;
        screenLight.DOIntensity(0, 5);
        DOTween.To(() => cam.m_Lens.Dutch, x => cam.m_Lens.Dutch = x, 120, .2f);
        GUI_Seen.gameObject.SetActive(false);
        GUI_Bar.gameObject.SetActive(false);
        cam.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.m_MaxSpeed = 0;
        cam.GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis.m_MaxSpeed = 0;
        playerRigidbody.constraints = RigidbodyConstraints.FreezeAll;
        
        yield return new WaitForSeconds(2);

        ScreenFlash.flash.Play();

        yield return new WaitForSeconds(10.1f);

        Application.Quit();
    }

    IEnumerator HideWaitTime()
    {
        if (hideCounter <= hideTimer)
        {
            hideCounter++;
            yield return new WaitForSeconds(1);
            StartCoroutine(HideWaitTime());
        }
        else
        {
            hidden = true;
            hideCounter = 0;
            stealthPoints += 50;
            DOTween.To(() => GUI_Seen.color, x => GUI_Seen.color = x, new Color(171, 171, 171, 0), 1.2f);
            yield return new WaitForSeconds(0);
        }
    }

    public void Spotted()
    {
        if (hidden)
        {
            DOTween.To(() => GUI_Seen.color, x => GUI_Seen.color = x, new Color(171,171,171,0.4f), 1.2f);
            stealthPoints -= 50;
            StartCoroutine(HideWaitTime());
        }

        hidden = false;
        hideCounter = 0;
    }

    public bool isHidden()
    {
        return hidden;
    }

    private void OnTriggerStay(Collider o)
    {
        if (o.CompareTag("Enemy"))
        {
            if (!nearEnemyList.Contains(o.transform))
                { nearEnemyList.Add(o.transform); }

            Transform nearestEnemy = nearEnemyList[0];

            foreach(Transform enemy in nearEnemyList)
            {
                if (Vector3.Distance(transform.position, enemy.position) < Vector3.Distance(transform.position, nearestEnemy.position))
                    nearestEnemy = enemy;
            }
            DistanceToStealth(nearestEnemy);
        }
    }

    void DistanceToStealth(Transform enemy)
    {
        int distanceValue = (int) Vector3.Distance(transform.position, enemy.position);
        if (distanceValue > 10)
            distanceValue = 10;
        int newStealthPoints = distanceValue * 5;

        if (newStealthPoints < distanceStealthPoints)
        {
            stealthPoints -= 5;
            distanceStealthPoints -= 5;
        }
        else if (newStealthPoints > distanceStealthPoints)
        {
            stealthPoints += 5;
            distanceStealthPoints += 5;
        }
        GUI_Bar.GetComponent<RectTransform>().localScale = new Vector3((10 - distanceValue) * 10, 100, 1);
    }

}