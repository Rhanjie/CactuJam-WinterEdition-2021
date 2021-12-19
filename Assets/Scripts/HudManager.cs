using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HudManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textScore;
    [SerializeField] private TextMeshProUGUI textHP;

    public void SetScore(float score)
    {
        textScore.text = $"Punkty: {score}";
    }

    public void SetHP(float hp, float maxHp)
    {
        textHP.text = $"Życie: {hp}/{maxHp}";
    }
}
