using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class UIScript : MonoBehaviour
{
    Button SettingButton;

    private void OnEnable()
    {
        var uiDocument = GetComponent<UIDocument>();
        var root = uiDocument.rootVisualElement;
        SettingButton = root.Q<Button>("SettingButton");
        SettingButton.clicked += SettingButton_Clicked;
    }
    private void SettingButton_Clicked()
    {
        SceneManager.LoadScene("SettingScene");
    }
}
