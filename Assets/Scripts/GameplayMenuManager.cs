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

    public void Init(bool isWin, int _score) {
        score = _score;
        
        textScore.text = $"Osiagnales wynik {score} punktow";

        if (isWin)
        {
            text.text = "Po wygranej Świętego Mikołaja nad paskudnym Dziadem Mrozem śnieżyce ustały, a potwory z koszmarów zniknęły!\n\n" +
                        "Nareszcie cały świat znowu poczuje ducha świąt, a dzieci dostaną swoje prezenty.\n\n" +
                        "Niestety dla Świętego Mikołata to dopiero początek pracy. Z powodu opóżnień ma bardzo mało czasu na dostarczenie wszystkich prezentów! Życz mu szczęścia, a być może odwiedzi Cię niebawem";
        }
        
        else text.text = "Niestety mimo wszelkich starań naszego drogiego Świętego Mikołaja, Dziadek Mróz i zastępy jego potwornych bałwanów zombie z robakami zwyciężyły.\n\n" +
                         "Od tego czasu gwiazdka nigdy już nie nadeszła, a dzieci przestały wierzyć w Świętego Mikołaja. Nadszedł smutny czas zapomnienia...";
    }
}
