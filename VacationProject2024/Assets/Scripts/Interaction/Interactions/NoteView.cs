using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteView : LockedInteraction
{
    [SerializeField] int noteIndex = 1;
    UIController uiController;
    protected override void Awake()
    {
        base.Awake();
        uiController = FindObjectOfType<UIController>();
    }
    protected override void OnUnlockedInteraction()
    {
        base.OnUnlockedInteraction();
        if (uiController != null) uiController.ShowSpecificPaperlist(noteIndex);
    }
}
