using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    public UiManager uiManager;
    private HudManager _hudManager;
    
    public Enemy boss;


    public int pointsToActivate = 50;


    private bool isShowed = false;
    private float timer = 0;
    private float secondsToHide = 15;

    private void Start()
    {
        _hudManager = uiManager.GetHudManager();
    }

    public void CheckLimit(int points)
    {
        if (!isShowed && points >= pointsToActivate)
        {
            pointsToActivate *= 2;
            
            ShowBoss();
        }
    }

    private void ShowBoss()
    {
        timer = 0;
        isShowed = true;
    }

    private void HideBoss()
    {
        timer = 0;
        isShowed = false;
    }

    private void Update()
    {
        if (isShowed)
        {
            timer += Time.deltaTime;

            if (timer >= secondsToHide)
            {
                HideBoss();
            }
        }
    }
}
