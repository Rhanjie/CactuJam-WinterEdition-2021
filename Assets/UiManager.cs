using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    [SerializeField] private Canvas hudCanvas;
    [SerializeField] private Canvas summaryPanelCanvas;

    [HideInInspector]
    public HudManager _hudManager;
    
    private void Start()
    {
        hudCanvas.gameObject.SetActive(true);
        summaryPanelCanvas.gameObject.SetActive(false);

        _hudManager = hudCanvas.GetComponent<HudManager>();
        
        ToggleMouse(false);

        AudioListener.volume = 0.5f;
    }

    public void OpenSummaryPanel()
    {
        summaryPanelCanvas.gameObject.SetActive(true);

        ToggleMouse(true);
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
