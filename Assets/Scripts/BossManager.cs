using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    public UiManager uiManager;
    private HudManager _hudManager;
    private Animator _animator;
    
    public Boss boss;
    
    public float pointsToActivate = 50;

    private float currentPointsToActivate;
    public float secondsToHide = 20;

    private bool isShowed = false;
    private float timer = 0;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _hudManager = uiManager.GetHudManager();
        
        timer = 0;
        isShowed = false;
        //boss.gameObject.SetActive(false);
        currentPointsToActivate = pointsToActivate;
        _hudManager.UpdateMaxScore(currentPointsToActivate);
    }

    public void CheckLimit(float points)
    {
        if (!isShowed && points >= currentPointsToActivate)
        {
            currentPointsToActivate += pointsToActivate;
            
            _hudManager.UpdateMaxScore(currentPointsToActivate);
            
            ShowBoss();
        }
    }

    private void ShowBoss()
    {
        timer = 0;
        isShowed = true;
        
        //boss.gameObject.SetActive(true);
        
        _animator.SetBool("isShowed", true);
        _hudManager.ShowBossHP();
    }

    private void HideBoss()
    {
        timer = 0;
        isShowed = false;
        
        //boss.gameObject.SetActive(false);
        
        _animator.SetBool("isShowed", false);
        _hudManager.HideBossHP();
    }

    private void Update()
    {
        _hudManager.UpdateBossHP(boss.HP, boss.maxHP);
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
