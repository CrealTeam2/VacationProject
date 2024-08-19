using UnityEngine;
using UnityEngine.UIElements;

public class ResolutionSettings : MonoBehaviour
{
    private VisualElement rootElement;

    void OnEnable()
    {
        // UI Document 컴포넌트를 통해 UXML 파일을 로드
        var uiDocument = GetComponent<UIDocument>();
        rootElement = uiDocument.rootVisualElement;

        // DropdownField 컴포넌트 가져오기
        var resolutionDropdown = rootElement.Q<DropdownField>("ResolutionDropdown");

        // 이벤트 핸들러 추가
        resolutionDropdown.RegisterValueChangedCallback(evt => OnResolutionChange(evt.newValue));
    }

    private void OnResolutionChange(string newResolution)
    {
        // 해상도 변경 로직 구현
        string[] res = newResolution.Split('x');
        int width = int.Parse(res[0]);
        int height = int.Parse(res[1]);

       
        Screen.SetResolution(width, height, FullScreenMode.Windowed);
    }
}
