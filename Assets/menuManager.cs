using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menuManager : MonoBehaviour {
    public void StartGame() {
        SceneManager.LoadScene("GameplayScene", LoadSceneMode.Single);
    }

    public void QuitGame() {
        Application.Quit();
    }
}
