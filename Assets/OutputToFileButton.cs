using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OutputToFileButton : MonoBehaviour
{
    [SerializeField] CoordinateReader coordinateReadear;
    [SerializeField] ToggleAbleCanvas startScreen;
    [SerializeField] ToggleAbleCanvas drawScreen;
    [SerializeField] TMP_InputField pathInputField;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {   
            coordinateReadear.gameObject.SetActive(true);
            coordinateReadear.SetITakePosition(new OutputToRapidModuleFile(pathInputField.text));
            startScreen.SetState(false);
            drawScreen.SetState(true);
        });
    }
}
