using UnityEngine.UIElements;
using UnityEngine;

public class SensitivitySettings : MonoBehaviour
{
    private Slider sensitivitySlider;
    private Label sensitivityLabel;


    private float defaultSensitivity = 1.0f;


    private float currentSensitivity;


    private float stepSize = 0.1f;

    private PlayerController playerController;

    private void OnEnable()
    {
        var uiDocument = GetComponent<UIDocument>();
        var root = uiDocument.rootVisualElement;


        sensitivitySlider = root.Q<Slider>("SensitivitySlider");
        sensitivityLabel = root.Q<Label>("SensitivityLabel");


        sensitivitySlider.lowValue = 0.1f;
        sensitivitySlider.highValue = 10.0f;
        sensitivitySlider.value = defaultSensitivity;


        currentSensitivity = PlayerPrefs.GetFloat("Sensitivity", defaultSensitivity);
        sensitivitySlider.value = currentSensitivity;

        playerController = FindObjectOfType<PlayerController>();

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


    private void ApplySensitivity(float sensitivity)
    {

        Debug.Log($"Sensitivity set to: {sensitivity}");
    }
}
