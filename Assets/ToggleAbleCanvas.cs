using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleAbleCanvas : MonoBehaviour
{
    CanvasGroup canvasGroup;
    
    private void OnEnable()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }
    public void toggleState(bool state)
    {
        canvasGroup.interactable = state;
        canvasGroup.blocksRaycasts = !state;
        canvasGroup.alpha = state? 1 : 0;
    }
}
