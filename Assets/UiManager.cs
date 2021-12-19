using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    [SerializeField] private Canvas hudCanvas;
    [SerializeField] private Canvas summaryPanelCanvas;
    
    private void Start()
    {
        hudCanvas.gameObject.SetActive(true);
        summaryPanelCanvas.gameObject.SetActive(true);
    }
}
