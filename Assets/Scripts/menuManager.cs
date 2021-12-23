using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class menuManager : MonoBehaviour {
    
    [SerializeField] private Slider volumeSlider;

    private void Start()
    {
        if (!PlayerPrefs.HasKey("GlobalVolume"))
        {
            volumeSlider.value = 0.5f;

            PlayerPrefs.SetFloat("GlobalVolume", 0.5f);
            PlayerPrefs.Save();
        }
        
        else volumeSlider.value =  PlayerPrefs.GetFloat("GlobalVolume");
        
        AudioListener.volume = volumeSlider.value;
        
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    
    public void UpdateVolume(float value)
    {
        AudioListener.volume = volumeSlider.value;
        PlayerPrefs.SetFloat("GlobalVolume", volumeSlider.value);
        PlayerPrefs.Save();
    }

    public void StartGame() {
        SceneManager.LoadScene("GameplayScene", LoadSceneMode.Single);
    }

    public void QuitGame() {
        Application.Quit();
    }
}
