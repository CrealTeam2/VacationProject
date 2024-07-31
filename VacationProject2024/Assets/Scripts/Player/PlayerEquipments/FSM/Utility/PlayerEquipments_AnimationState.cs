using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipments_AnimationState : State<PlayerEquipments>
{
    string clipName, endStateName;
    public PlayerEquipments_AnimationState(PlayerEquipments origin, Layer<PlayerEquipments> parent, string clipName, string endStateName) : base(origin, parent)
    {
        this.clipName = clipName;
        this.endStateName = endStateName;
    }
    public override void OnStateEnter()
    {
        base.OnStateEnter();
        origin.anim.Play(clipName);
        origin.onClipFinish += ClipFinish;
    }
    void ClipFinish()
    {
        parentLayer.ChangeState(endStateName);
    }
    public override void OnStateExit()
    {
        base.OnStateExit();
        origin.onClipFinish -= ClipFinish;
    }
}
