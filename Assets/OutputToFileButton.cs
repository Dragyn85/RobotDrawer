using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OutputToFileButton : MonoBehaviour
{
    [SerializeField] CoordinateReader coordinateReadear;
    [SerializeField] ToggleAbleCanvas startScreen;
    [SerializeField] ToggleAbleCanvas drawScreen;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            coordinateReadear.SetITakePosition(new OutputToRapidModuleFile());
            startScreen.SetState(false);
            drawScreen.SetState(true);
        });
    }
}
