using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerEquipments_AnimationState : State<PlayerEquipments>
{
    string clipName;
    public PlayerEquipments_AnimationState(PlayerEquipments origin, Layer<PlayerEquipments> parent, string clipName) : base(origin, parent)
    {
        this.clipName = clipName;
    }
    public override void OnStateEnter()
    {
        base.OnStateEnter();
        origin.anim.Play(clipName);
        origin.onClipFinish += ClipFinish;
    }
    protected virtual void ClipFinish()
    {
        
    }
    public override void OnStateExit()
    {
        base.OnStateExit();
        origin.onClipFinish -= ClipFinish;
    }
}
