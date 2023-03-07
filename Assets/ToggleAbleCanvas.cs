using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleAbleCanvas : MonoBehaviour
{
    CanvasGroup canvasGroup;
    [SerializeField] bool startActive;
    
    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        SetState(startActive);
    }
    public void SetState(bool state)
    {
        canvasGroup.interactable = state;
        canvasGroup.blocksRaycasts = state;
        canvasGroup.alpha = state? 1 : 0;
    }
}
