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
        iconDictionary.Clear();
        rootVisualElement = document.rootVisualElement;
        foreach (var key in keysForIcon)
            iconDictionary.Add(key, CreateIcon(key));

        DontDestroyOnLoad(document.transform.parent.gameObject);

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
        label.style.color = Color.white;    
        label.style.backgroundColor = new Color(0, 0, 0, 0);
        label.style.borderBottomColor = new Color(0, 0, 0, 0);
        label.style.borderTopColor = new Color(0, 0, 0, 0);
        label.style.borderLeftColor = new Color(0, 0, 0, 0);
        label.style.borderRightColor = new Color(0,0,0,0);
        label.style.display = DisplayStyle.None;

        return label;
    }

    public Icon GetIcon(KeyCode key, int size) //�������� ���
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

    public void RetrunIcon(ref Icon icon) //����� �������� �ݳ�
    {
        icon.isUsing = false;
        icon.Disable();
        icon = null;
    }
}
