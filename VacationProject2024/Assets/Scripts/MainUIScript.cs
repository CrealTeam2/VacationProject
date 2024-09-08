using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIController : MonoBehaviour
{
    private Button SettingButton;
    private Button ExitButton;

    private VisualElement StartScene;
    private VisualElement SettingScene;

    private Button SceneSetting;
    private Button ControlSetting;
    private Button SoundSetting;
    private Button GameStart;
    private VisualElement Panel1;
    private VisualElement Panel2;
    private VisualElement Panel3;
    DropdownField SceneControl;
    private Slider sensitivitySlider;
    private Label sensitivityLabel;
    private float defaultSensitivity = 1.0f;
    private float currentSensitivity;
    private float stepSize = 0.1f;
    private Player playerController;
    private VisualElement TabUI;
    private bool isTabUIVisible = false;

    private void OnEnable()
    {
        var uiDocument = GetComponent<UIDocument>();
        var root = uiDocument.rootVisualElement;

        SettingButton = root.Q<Button>("SettingButton");
        ExitButton = root.Q<Button>("ExitButton");

        StartScene = root.Q<VisualElement>("StartScene");
        SettingScene = root.Q<VisualElement>("SettingScene");

        sensitivitySlider = root.Q<Slider>("sensitivitySlider");
        sensitivityLabel = root.Q<Label>("sensitivityLabel");
        GameStart = root.Q<Button>("GameStart");
        GameStart.clicked += GameStartButton_Clicked;

        ShowStartScene();

        SettingButton.clicked += OnSettingButtonClicked;
        ExitButton.clicked += OnExitButtonClicked;

        SceneSetting = root.Q<Button>("SceneSetting");
        ControlSetting = root.Q<Button>("ControlSetting");
        SoundSetting = root.Q<Button>("SoundSetting");

        Panel1 = root.Q<VisualElement>("Panel1");
        Panel2 = root.Q<VisualElement>("Panel2");
        Panel3 = root.Q<VisualElement>("Panel3");

        TabUI = root.Q<VisualElement>("TabUI");
        TabUI.style.display = DisplayStyle.None;

        SceneSetting.clicked += ShowPanel1;
        ControlSetting.clicked += ShowPanel2;
        SoundSetting.clicked += ShowPanel3;

        SceneControl = root.Q<DropdownField>("SceneControl");
        SceneControl.choices = new List<string>
        {
            "1920 x 1080 (16:9)",
            "1600 x 900",
            "1280 x 720"
        };
        SceneControl.value = SceneControl.choices[0];
        SceneControl.RegisterValueChangedCallback(evt =>
        {
            ChangeResolution(evt.newValue);
        });

        sensitivitySlider.lowValue = 0.1f;
        sensitivitySlider.highValue = 10.0f;
        sensitivitySlider.value = defaultSensitivity;

        currentSensitivity = PlayerPrefs.GetFloat("Sensitivity", defaultSensitivity);
        sensitivitySlider.value = currentSensitivity;

        playerController = FindObjectOfType<Player>();

        if (playerController != null)
        {
            playerController.SetMovementEnabled(false);
        }

        sensitivitySlider.RegisterValueChangedCallback(evt =>
        {
            float newValue = Mathf.Round(evt.newValue / stepSize) * stepSize;
            sensitivitySlider.value = newValue;
            currentSensitivity = newValue;
            sensitivityLabel.text = $"���� ��: {currentSensitivity:F1}";
            ApplySensitivity(currentSensitivity);

            PlayerPrefs.SetFloat("Sensitivity", currentSensitivity);
        });

        sensitivityLabel.text = $"���� ��: {currentSensitivity:F1}";
    }

    private void ChangeResolution(string resolution)
    {
        switch (resolution)
        {
            case "1920 x 1080 (16:9)":
                Screen.SetResolution(1920, 1080, FullScreenMode.FullScreenWindow);
                Debug.Log("Resolution changed to 1920 x 1080");
                break;
            case "1600 x 900":
                Screen.SetResolution(1600, 900, FullScreenMode.FullScreenWindow);
                Debug.Log("Resolution changed to 1600 x 900");
                break;
            case "1280 x 720":
                Screen.SetResolution(1280, 720, FullScreenMode.FullScreenWindow);
                Debug.Log("Resolution changed to 1280 x 720");
                break;
            default:
                Debug.LogWarning("Unsupported resolution selected.");
                break;
        }
    }

    private void GameStartButton_Clicked()
    {
        if (playerController != null)
        {
            playerController.SetMovementEnabled(true);

        }
        StartScene.style.display = DisplayStyle.None;
    }

    private void ShowStartScene()
    {
        StartScene.style.display = DisplayStyle.Flex;
        SettingScene.style.display = DisplayStyle.None;
    }

    private void OnSettingButtonClicked()
    {
        StartScene.style.display = DisplayStyle.None;
        SettingScene.style.display = DisplayStyle.Flex;
        ShowPanel1();
    }

    private void OnExitButtonClicked()
    {
        ShowStartScene();
    }

    private void ShowPanel1()
    {
        Panel1.style.display = DisplayStyle.Flex;
        Panel2.style.display = DisplayStyle.None;
        Panel3.style.display = DisplayStyle.None;
    }

    private void ShowPanel2()
    {
        Panel1.style.display = DisplayStyle.None;
        Panel2.style.display = DisplayStyle.Flex;
        Panel3.style.display = DisplayStyle.None;
    }

    private void ShowPanel3()
    {
        Panel1.style.display = DisplayStyle.None;
        Panel2.style.display = DisplayStyle.None;
        Panel3.style.display = DisplayStyle.Flex;
    }

    private void ApplySensitivity(float sensitivity)
    {
        if (playerController != null)
        {
            playerController.UpdateSensitivity(sensitivity);
        }
        Debug.Log($"Sensitivity set to: {sensitivity}");
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleTabUI();
        }
    }

    private void ToggleTabUI()
    {
        isTabUIVisible = !isTabUIVisible;
        TabUI.style.display = isTabUIVisible ? DisplayStyle.Flex : DisplayStyle.None;

        if (playerController != null)
        {

            playerController.SetMovementEnabled(!isTabUIVisible);
        }
    }
}