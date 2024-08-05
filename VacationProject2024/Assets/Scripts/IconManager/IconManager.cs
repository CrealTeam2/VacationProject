using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public enum ScadulingPriority { Null, Low, Medium, High }
public class IconManager : Singleton<IconManager>
{
    Dictionary<KeyCode, Icon> iconDictionary = new Dictionary<KeyCode, Icon>();
    VisualElement rootVisualElement;
    public UIDocument document;
    [SerializeField] List<KeyCode> keysForIcon = new();

    private void Awake()
    {
        rootVisualElement = document.rootVisualElement;
        foreach (var key in keysForIcon)
            iconDictionary.Add(key, CreateIcon(key));
        
    }
    Icon icon;
    private void Start()
    {
        icon = GetIcon(KeyCode.F, 80);
        icon.Enable();

        icon.style.top = 500;
        icon.style.left = 500;

        Invoke("A", 4);
    }
    void A()
    {
        RetrunIcon(ref icon);
    }

    Icon CreateIcon(KeyCode key)
    {
        var label = new Icon(key);
        rootVisualElement.Add(label);
        label.text = key.ToString();
        label.style.unityTextAlign = TextAnchor.MiddleCenter;
        label.style.fontSize = 80;
        label.style.minWidth = 80;
        label.style.height = 80;
        label.style.position = Position.Absolute;
        label.style.backgroundColor = new Color(1, 1, 1, 0.2f);
        label.style.borderBottomColor = Color.black;
        label.style.borderTopColor = Color.black;
        label.style.borderLeftColor = Color.black;
        label.style.borderRightColor = Color.black;
        label.style.borderTopWidth = 5;
        label.style.borderBottomWidth = 5;
        label.style.borderLeftWidth = 5;
        label.style.borderRightWidth = 5;
        label.style.borderBottomLeftRadius = 10;
        label.style.borderBottomRightRadius = 10;
        label.style.borderTopLeftRadius = 10;
        label.style.borderTopRightRadius = 10;
        label.style.display = DisplayStyle.None;

        return label;
    }

    public Icon GetIcon(KeyCode key, int size) //아이콘을 사용
    {
        iconDictionary.TryGetValue(key, out Icon icon);
        if (icon == null)
        {
            Debug.LogWarning("There is no Icon for required Key");
            return null;
        }
        if (icon.isUsing == true)
        {
            Debug.LogWarning("This Icon is already being used");
            return null;
        }
        icon.style.fontSize = size;
        icon.style.minWidth = size;
        icon.style.height = size;
        icon.isUsing = true;
        return icon;
    }

    public void RetrunIcon(ref Icon icon) //사용한 아이콘을 반납
    {
        icon.isUsing = false;
        icon.Disable();
        icon = null;
    }
}
