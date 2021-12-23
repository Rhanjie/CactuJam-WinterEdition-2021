using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HudManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textScore;
    [SerializeField] private TextMeshProUGUI textHP;

    [SerializeField] private Animator bossSection;
    [SerializeField] private Slider bossHPSlider;

    private float _score;
    private float _maxScore;

    public void SetScore(float score)
    {
        _score = score;
        
        textScore.text = $"Punkty: {_score}/{_maxScore}";
    }

    public void UpdateMaxScore(float maxScore)
    {
        _maxScore = maxScore;
        
        textScore.text = $"Punkty: {_score}/{_maxScore}";
    }

    public void SetHP(float hp, float maxHp)
    {
        textHP.text = $"Życie: {hp}/{maxHp}";
    }

    public void UpdateBossHP(float hp, float maxHp)
    {
        bossHPSlider.value = 1 - (hp / maxHp);
    }

    public void ShowBossHP()
    {
        bossSection.SetTrigger("Show");
    }
    
    public void HideBossHP()
    {
        bossSection.SetTrigger("Hide");
    }
}
