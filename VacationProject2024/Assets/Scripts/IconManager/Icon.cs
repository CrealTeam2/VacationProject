using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEngine;

public class Icon : Label
{
    public KeyCode key;
    public bool isUsing;

    public Icon(KeyCode key)
    {
        this.key = key;
        isUsing = false;
    }

    public void Enable()
    {
        style.display = DisplayStyle.Flex;
    }

    public void Disable()
    {
        style.display = DisplayStyle.None;
    }
}

