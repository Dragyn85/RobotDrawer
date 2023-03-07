using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ConnectButton : MonoBehaviour
{
    [SerializeField] List<ValidateConnectionSetting> connectionsValidationComponents;
        [SerializeField] TMP_InputField iPInputField;
    [SerializeField] TMP_InputField portInputField;
    [SerializeField] SendDataToRobot robotConnection;
    [SerializeField] ConnectionPrompt ConnectionPromt;
    [SerializeField] CoordinateReader coordinateReader;

    public UnityEvent OnConnectedToRobot;

    
    Button button;

    private void OnEnable()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(Connect);
        foreach (var connection in connectionsValidationComponents)
        {
            connection.ValidationChanged += CheckValidations;
        }
    }

    private void CheckValidations()
    {
        bool valid =connectionsValidationComponents.All(t => t.IsValid());
        ActivateButton(valid);
    }

    public void ActivateButton(bool state)
    {
        button.interactable = state;
    }
    void Connect()
    {
        robotConnection.ConnectToRobot(iPInputField.text, portInputField.text, OnConnectionFinish);
        ConnectionPromt.gameObject.SetActive(true);
        ConnectionPromt.SetText("Connecting to Robot, please wait!");
        ActivateButton(false);
        coordinateReader.SetITakePosition(robotConnection);
    }
    void OnConnectionFinish(ConnectionResults result)
    {
        ActivateButton(true);
        if (result.success)
        {
            OnConnectedToRobot.Invoke();

            ConnectionPromt.gameObject.SetActive(false);
        }
        else
        {
            ConnectionPromt.SetText("Unable to connect to robotcontroller");
            StartCoroutine(HideAfterDelay());
        }
    }
    IEnumerator HideAfterDelay()
    {
        yield return new WaitForSeconds(2);
        ConnectionPromt.gameObject.SetActive(false);
    }

}

public class ConnectionResults
{
    public bool success;
    public ConnectionResults(bool success)
    {
        this.success = success;
    }
}
