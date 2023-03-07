using SFB;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectOutputButton : MonoBehaviour
{
    [SerializeField] TMP_InputField pathTextInputField;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            var path = StandaloneFileBrowser.OpenFolderPanel("Select Output folder", pathTextInputField.text, false);

            if (path.Length > 0)
            {
                pathTextInputField.text = path[0];
                PlayerPrefs.SetString("OutputPath", path[0]);
            };
        });
    }
}
