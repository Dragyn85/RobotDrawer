using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class outputPathVisual : MonoBehaviour
{
    [SerializeField] TMP_InputField outputPathText;
    // Start is called before the first frame update
    void Start()
    {
        var path = PlayerPrefs.GetString("OutputPath");
        if (path.Length == 0)
        {
            path = Application.persistentDataPath;
        }
        
        path = path.Replace("/", "\\");
        outputPathText.text = path;
    }
}
