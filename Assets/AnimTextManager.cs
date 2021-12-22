using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AnimTextManager : MonoBehaviour
{
    public TextMeshProUGUI textPrefab;
    public float speed = 0.5f;

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
        for (int i =  0; i < addedTexts.Count; i++)
        {
            var color = addedTexts[i].color;
            
            addedTexts[i].transform.position += speed * Vector3.up * Time.deltaTime;
            color.a -= speed * Time.deltaTime;
            
            addedTexts[i].color = color;
            if (color.a <= 0)
            {
                Destroy(addedTexts[i].gameObject);
                addedTexts.Remove(addedTexts[i]);

                i -= 1;
            }
        }
    }
}
