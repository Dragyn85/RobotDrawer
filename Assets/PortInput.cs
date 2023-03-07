using TMPro;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using System;

public class PortInput : ValidateConnectionSetting
{
    TMP_InputField inputField;
    Image image;
    private bool validInput;

    

    private void OnEnable()
    {
        image = GetComponent<Image>();
        inputField = GetComponent<TMP_InputField>();
        inputField.onEndEdit.AddListener(HandleInputChanged);
    }

    void HandleInputChanged(string text)
    {
        CheckInputText(text);
        ValidationChanged?.Invoke();
        UpdateUI(validInput);
    }

    void CheckInputText(string newText)
    {
        validInput = IsStringValidPort(newText);
    }

    bool IsStringValidPort(string portString)
    {
        if (!int.TryParse(portString, out int portnumber))
            return false;
        if(portnumber > 9999)
            return false;

        return true;
    }

    void UpdateUI(bool validIP)
    {
        image.color = validIP ? Color.white : Color.red;
    }

    public override bool IsValid()
    {
        CheckInputText(inputField.text);
        
        return validInput;
    }
}
