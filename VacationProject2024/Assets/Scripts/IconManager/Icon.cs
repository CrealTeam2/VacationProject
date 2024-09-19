using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEngine;
using System.IO;

public class Icon : Label
{
    public KeyCode key;
    public bool isUsing;

    public Icon(KeyCode key)
    {
        this.key = key;
        isUsing = false;
        style.unityFontDefinition = new StyleFontDefinition(FontDefinition.FromFont(Resources.Load<Font>(Path.Combine("font", "InteractionFont"))));
    }

    public void Enable()    
    {
        style.display = DisplayStyle.Flex;
    }

    public void Disable()
    {
        style.display = DisplayStyle.None;
    }

    public void SetPosition(float x, float y)
    {
        Debug.Log(Screen.currentResolution.width + " " + Screen.currentResolution.height);
        style.left = (x - (resolvedStyle.width / 2)) * (1920f / Screen.width);
        style.bottom = (y - (resolvedStyle.height / 2)) * (1080f / Screen.height);
    }
    public void SetText(string str)
    {
        text = str;
    }
}

