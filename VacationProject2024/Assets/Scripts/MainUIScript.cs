using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIController : MonoBehaviour
{
    private Button SettingButton;
    private Button ExitButton;
    private Button ProfileButton;
    private Button Cencel;
    private Button SceneSetting;
    private Button ControlSetting;
    private Button SoundSetting;
    private Button GameStart;
    private Button ReGameStart2;
    private Button GameSetting;
    private Button GameOut;
    private Button Cencel3;
    private Button Cencel4;
    private Button Cencel5;
    private VisualElement StartScene;
    private VisualElement SettingScene;
    private VisualElement Controlview;
    private VisualElement EscView;
    private VisualElement Panel1;
    private VisualElement Panel2;
    private VisualElement Panel3;
    private DropdownField SceneControl;
    private Slider sensitivitySlider;
    private Label sensitivityLabel;
    private Label PaperText;
    private Label PaperText2;
    private Label PaperText3;
    private Label PaperText4;
    private VisualElement TabUI;
    private Button Cencel2;
    private VisualElement Paperlist;
    private VisualElement Paperlist2;
    private VisualElement Paperlist3;
    private VisualElement Paperlist4;

    private float defaultSensitivity = 1.0f;
    private float currentSensitivity;
    private float stepSize = 0.1f;

    private PlayerController playerController;
    private bool isTabUIVisible = false;
    private bool isGamePaused = false;

    private void OnEnable()
    {
        InitializeUIElements();
        InitializeSettings();
    }

    private void InitializeUIElements()
    {
        var uiDocument = GetComponent<UIDocument>();
        if (uiDocument == null)
        {
            Debug.LogError("UIDocument component not found on the GameObject.");
            return;
        }

        var root = uiDocument.rootVisualElement;

        SettingButton = root.Q<Button>("SettingButton");
        ExitButton = root.Q<Button>("ExitButton");
        ProfileButton = root.Q<Button>("ProfileButton");
        Cencel = root.Q<Button>("Cencel");
        SceneSetting = root.Q<Button>("SceneSetting");
        ControlSetting = root.Q<Button>("ControlSetting");
        SoundSetting = root.Q<Button>("SoundSetting");
        GameStart = root.Q<Button>("GameStart");
        ReGameStart2 = root.Q<Button>("ReGameStart2");
        GameSetting = root.Q<Button>("GameSetting");
        GameOut = root.Q<Button>("GameOut");
        StartScene = root.Q<VisualElement>("StartScene");
        SettingScene = root.Q<VisualElement>("SettingScene");
        Controlview = root.Q<VisualElement>("Controlview");
        EscView = root.Q<VisualElement>("EscView");
        Panel1 = root.Q<VisualElement>("Panel1");
        Panel2 = root.Q<VisualElement>("Panel2");
        Panel3 = root.Q<VisualElement>("Panel3");
        SceneControl = root.Q<DropdownField>("SceneControl");
        sensitivitySlider = root.Q<Slider>("sensitivitySlider");
        sensitivityLabel = root.Q<Label>("sensitivityLabel");
        TabUI = root.Q<VisualElement>("TabUI");
        Cencel2 = root.Q<Button>("Cencel2");
        Paperlist = root.Q<VisualElement>("Paperlist");
        Paperlist2 = root.Q<VisualElement>("Paperlist2");
        Paperlist3 = root.Q<VisualElement>("Paperlist3");
        Paperlist4 = root.Q<VisualElement>("Paperlist4");
        Cencel3 = root.Q<Button>("Cencel3");
        Cencel4 = root.Q<Button>("Cencel4");
        Cencel5 = root.Q<Button>("Cencel5");
        PaperText = root.Q<Label>("PaperText");
        PaperText2 = root.Q<Label>("PaperText2");
        PaperText2 = root.Q<Label>("PaperText3");
        PaperText2 = root.Q<Label>("PaperText4");

        if (TabUI != null) TabUI.style.display = DisplayStyle.None;
        if (Paperlist != null) Paperlist.style.display = DisplayStyle.None;
        if (Cencel2 != null) Cencel2.clicked += () => OnCencelClicked(Paperlist);
        if (Cencel3 != null) Cencel3.clicked += () => OnCencelClicked(Paperlist2);
        if (Cencel4 != null) Cencel4.clicked += () => OnCencelClicked(Paperlist3);
        if (Cencel5 != null) Cencel5.clicked += () => OnCencelClicked(Paperlist4);
        if (Paperlist2 != null) Paperlist2.style.display = DisplayStyle.None;
        if (Paperlist3 != null) Paperlist3.style.display = DisplayStyle.None;
        if (Paperlist4 != null) Paperlist4.style.display = DisplayStyle.None;

        if (GameStart != null) GameStart.clicked += GameStartButton_Clicked;
        if (SettingButton != null) SettingButton.clicked += OnSettingButtonClicked;
        if (ExitButton != null) ExitButton.clicked += OnExitButtonClicked;
        if (SceneSetting != null) SceneSetting.clicked += ShowPanel1;
        if (ControlSetting != null) ControlSetting.clicked += ShowPanel2;
        if (SoundSetting != null) SoundSetting.clicked += ShowPanel3;
        if (ProfileButton != null) ProfileButton.clicked += OnProfileButtonClicked;
        if (Cencel != null) Cencel.clicked += OnCencelClicked;
        if (ReGameStart2 != null) ReGameStart2.clicked += OnReGameStart2Clicked;
        if (GameSetting != null) GameSetting.clicked += OnGameSettingClicked;
        if (GameOut != null) GameOut.clicked += OnGameOutClicked;

        ShowStartScene();
        Controlview.style.display = DisplayStyle.None;
        EscView.style.display = DisplayStyle.None;
    }

    private void InitializeSettings()
    {
        if (SceneControl != null)
        {
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
        }

        if (sensitivitySlider != null)
        {
            sensitivitySlider.lowValue = 0.1f;
            sensitivitySlider.highValue = 10.0f;
            sensitivitySlider.value = defaultSensitivity;

            currentSensitivity = PlayerPrefs.GetFloat("Sensitivity", defaultSensitivity);
            sensitivitySlider.value = currentSensitivity;

            sensitivitySlider.RegisterValueChangedCallback(evt =>
            {
                float newValue = Mathf.Round(evt.newValue / stepSize) * stepSize;
                sensitivitySlider.value = newValue;
                currentSensitivity = newValue;
                sensitivityLabel.text = $"감도 값: {currentSensitivity:F1}";
                ApplySensitivity(currentSensitivity);

                PlayerPrefs.SetFloat("Sensitivity", currentSensitivity);
            });

            sensitivityLabel.text = $"감도 값: {currentSensitivity:F1}";
        }

        playerController = FindObjectOfType<PlayerController>();
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
        ResumeGame();
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

    public void ShowSpecificPaperlist(int paperNumber)
    {
        Paperlist.style.display = DisplayStyle.None;
        Paperlist2.style.display = DisplayStyle.None;
        Paperlist3.style.display = DisplayStyle.None;
        Paperlist4.style.display = DisplayStyle.None;

        switch (paperNumber)
        {
            case 1:
                Paperlist.style.display = DisplayStyle.Flex;
                break;
            case 2:
                Paperlist2.style.display = DisplayStyle.Flex;
                break;
            case 3:
                Paperlist3.style.display = DisplayStyle.Flex;
                break;
            case 4:
                Paperlist4.style.display = DisplayStyle.Flex;
                break;
            default:
                Debug.LogWarning("Invalid Paperlist number.");
                break;
        }
    }

    private void OnCencel2Clicked()
    {
        HideAllPaperlists();
    }

    private void HideAllPaperlists()
    {
        if (Paperlist != null) Paperlist.style.display = DisplayStyle.None;
        if (Paperlist2 != null) Paperlist2.style.display = DisplayStyle.None;
        if (Paperlist3 != null) Paperlist3.style.display = DisplayStyle.None;
        if (Paperlist4 != null) Paperlist4.style.display = DisplayStyle.None;
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
    }

    private void OnProfileButtonClicked() {
        if (Controlview != null)
        {
            Controlview.style.display = DisplayStyle.Flex;
        }
        if (StartScene != null)
        {
            StartScene.style.display = DisplayStyle.None;
        }
    }
    private void OnCencelClicked() {
        if (Controlview != null)
        {
            Controlview.style.display = DisplayStyle.None;
        }
        if (StartScene != null)
        {
            StartScene.style.display = DisplayStyle.Flex;
        }
    }
    private void OnReGameStart2Clicked() {
        if (EscView != null)
        {
            EscView.style.display = DisplayStyle.None;
        }
        ResumeGame();
        isGamePaused = false;
    }
    private void OnGameSettingClicked() {
        if (EscView != null)
        {
            EscView.style.display = DisplayStyle.None;
        }
        if (SettingScene != null)
        {
            SettingScene.style.display = DisplayStyle.Flex;
        }
    }
    private void OnGameOutClicked() { Application.Quit(); }

    private void PauseGame()
    {
        Time.timeScale = 0f;
        if (playerController != null)
        {
            playerController.SetMovementEnabled(false);
        }
    }

    private void ResumeGame()
    {
        isGamePaused = false;
        Time.timeScale = 1f;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleTabUI();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleEscView();
        }
    }
    private void ToggleTabUI()
    {
        isTabUIVisible = !isTabUIVisible;

        if (isTabUIVisible)
        {
            if (TabUI != null)
            {
                TabUI.style.display = DisplayStyle.Flex;
            }
            if (playerController != null)
            {
                playerController.SetMovementEnabled(false);
            }
        }
        else
        {
            if (TabUI != null)
            {
                TabUI.style.display = DisplayStyle.None;
            }
            if (playerController != null)
            {
                playerController.SetMovementEnabled(true);
            }
        }
    }

    private void ToggleEscView()
    {
        if (isGamePaused)
        {
            if (EscView != null)
            {
                EscView.style.display = DisplayStyle.None;
            }
            ResumeGame();
        }
        else
        {
            if (EscView != null)
            {
                EscView.style.display = DisplayStyle.Flex;
            }
            PauseGame();
        }
        isGamePaused = !isGamePaused;
    }
    private void OnCencelClicked(VisualElement paperlist)
    {
        if (paperlist != null)
        {
            paperlist.style.display = DisplayStyle.None;
        }
    }
}
