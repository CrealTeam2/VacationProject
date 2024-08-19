using UnityEngine;
using UnityEngine.UIElements;

public class ResolutionSettings : MonoBehaviour
{
    private VisualElement rootElement;

    void OnEnable()
    {
        // UI Document ������Ʈ�� ���� UXML ������ �ε�
        var uiDocument = GetComponent<UIDocument>();
        rootElement = uiDocument.rootVisualElement;

        // DropdownField ������Ʈ ��������
        var resolutionDropdown = rootElement.Q<DropdownField>("ResolutionDropdown");

        // �̺�Ʈ �ڵ鷯 �߰�
        resolutionDropdown.RegisterValueChangedCallback(evt => OnResolutionChange(evt.newValue));
    }

    private void OnResolutionChange(string newResolution)
    {
        // �ػ� ���� ���� ����
        string[] res = newResolution.Split('x');
        int width = int.Parse(res[0]);
        int height = int.Parse(res[1]);

       
        Screen.SetResolution(width, height, FullScreenMode.Windowed);
    }
}
