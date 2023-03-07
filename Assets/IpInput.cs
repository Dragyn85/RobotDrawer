using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;
using System;
using UnityEngine.UI;

public class IpInput : ValidateConnectionSetting
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

    void HandleInputChanged(string newText)
    {
        CheckInputText(newText);
        ValidationChanged?.Invoke();
        UpdateUI(validInput);
    }

    private void CheckInputText(string newText)
    {
        validInput = IsStringValidIpAdress(newText);
    }

    private bool IsStringValidIpAdress(string newText)
    {
        var ipParts = newText.Split('.');

        bool partsPassCheck = ipParts.All(t => 
            t.Length < 4 && 
            int.TryParse(t, out int part) && 
            part < 256);

        return partsPassCheck && ContainsThreeDots(newText);
    }

    private static bool ContainsThreeDots(string newText)
    {
        var numberOfDots = newText.Count(c => c == '.');
        return numberOfDots == 3;
    }

    private void UpdateUI(bool validIP)
    {
        image.color = validIP ? Color.white : Color.red;
    }

    public override bool IsValid()
    {
        CheckInputText(inputField.text);
        return validInput;
    }
}
