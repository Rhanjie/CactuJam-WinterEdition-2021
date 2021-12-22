using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AnimTextManager : MonoBehaviour
{
    public TextMeshProUGUI textPrefab;

    private List<TextMeshProUGUI> addedTexts;

    private void Start()
    {
        addedTexts = new List<TextMeshProUGUI>();
    }

    public void PushText(float score)
    {
        if (score == 0)
            return;
        
        var textMeshProUGUI = Instantiate(textPrefab, transform);
        textMeshProUGUI.text = score > 1
            ? $"Multikill! +{score}"
            : $"+{score}";
        
        addedTexts.Add(textMeshProUGUI);
    }

    private void Update()
    {
        foreach (var addedText in addedTexts)
        {
            addedText.transform.position += Vector3.up * Time.deltaTime;
            var color = addedText.color;
            color.a -= Time.deltaTime * 1;
            
            addedText.color = color;

            if (color.a <= 0)
            {
                //Destroy(addedText.gameObject);
                //addedTexts.Remove(addedText);
            }
        }
    }
}
