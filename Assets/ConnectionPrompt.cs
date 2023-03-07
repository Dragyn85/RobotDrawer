using TMPro;
using UnityEngine;

public class ConnectionPrompt : MonoBehaviour
{
    TMP_Text text;

    public void SetText(string text)
    {
        this.text.SetText(text);
    }
    private void Awake()
    {
        text = GetComponent<TMP_Text>();
    }


}