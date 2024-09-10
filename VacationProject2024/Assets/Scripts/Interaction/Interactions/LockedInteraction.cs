using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedInteraction : InteractionAgent
{
    [SerializeField] string lockedFeedback;
    string original;
    protected bool unlocked = false;
    protected override void Awake()
    {
        base.Awake();
        original = feedbackText;
        feedbackText = lockedFeedback;
    }
    public void Unlock()
    {
        if (unlocked) return;
        feedbackText = original;
        unlocked = true;
        OnUnlock();
    }
    protected virtual void OnUnlock()
    {

    }
    public override void UpdateUnitFromVariable(ref DataUnit du)
    {
        du.Bool["Unlocked"] = unlocked;
    }
    public override void UpdateVariableFromUnit(DataUnit du)
    {
        bool tmp;  du.Bool.TryGetValue("Unlocked", out tmp);
        if (tmp)
        {
            Unlock();
        }
    }
    public override sealed void OnInteraction()
    {
        if (!unlocked) return;
        base.OnInteraction();
        OnUnlockedInteraction();
    }
    protected virtual void OnUnlockedInteraction()
    {

    }
}
