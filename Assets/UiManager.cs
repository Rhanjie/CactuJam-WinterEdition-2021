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
    }

    public void OpenSummaryPanel()
    {
        summaryPanelCanvas.gameObject.SetActive(true);
    }

    public HudManager GetHudManager()
    {
        return _hudManager;
    }
}
