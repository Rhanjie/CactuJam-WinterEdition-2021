using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayMenuManager : MonoBehaviour {
    public TextMeshProUGUI text;
    
    public TextMeshProUGUI textScore;
    
    private int score = 0;

    public void Init(int _score) {
        score = _score;

        textScore.text = $"Osiagnales wynik {score} punktow";
        
        text.text = "Niestety dziadek mroz wygral...";
    }
    public void BackToMenu() {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("MenuScene", LoadSceneMode.Single);
    }
}
