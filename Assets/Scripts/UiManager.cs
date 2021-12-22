using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UiManager : MonoBehaviour
{
    [SerializeField] private Canvas hudCanvas;
    [SerializeField] private Canvas summaryPanelCanvas;
    [SerializeField] private Canvas menuPanelCanvas;

    [HideInInspector]
    public HudManager _hudManager;
    
    private void Start()
    {
        AudioListener.volume = 0.5f;
        Time.timeScale = 1.0f;
        
        hudCanvas.gameObject.SetActive(true);
        summaryPanelCanvas.gameObject.SetActive(false);
        menuPanelCanvas.gameObject.SetActive(false);

        _hudManager = hudCanvas.GetComponent<HudManager>();
        
        ToggleMouse(false);
    }

    public void OpenSummaryPanel()
    {
        Time.timeScale = 0.0f;
        
        summaryPanelCanvas.gameObject.SetActive(true);

        ToggleMouse(true);
    }
    
    public void OpenMenuPanel()
    {
        Time.timeScale = 0.0f;
        
        menuPanelCanvas.gameObject.SetActive(true);

        ToggleMouse(true);
    }

    public void CloseMenuPanel()
    {
        Time.timeScale = 1.0f;
        
        menuPanelCanvas.gameObject.SetActive(false);
        
        ToggleMouse(false);
    }
    
    public void BackToMenu() {
        Time.timeScale = 1.0f;
        
        SceneManager.LoadScene("MenuScene", LoadSceneMode.Single);
    }

    public HudManager GetHudManager()
    {
        return _hudManager;
    }

    private void ToggleMouse(bool show)
    {
        Cursor.lockState = show ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = show;
    }
}
