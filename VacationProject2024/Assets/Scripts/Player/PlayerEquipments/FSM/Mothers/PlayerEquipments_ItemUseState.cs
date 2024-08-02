using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerEquipments_ItemUseState : PlayerEquipments_AnimationState
{
    public PlayerEquipments_ItemUseState(PlayerEquipments origin, Layer<PlayerEquipments> parent, string clip) : base(origin, parent, clip)
    {

    }
    protected override void ClipFinish()
    {
        base.ClipFinish();
        if(origin.switchingTo == 0)
        {
            parentLayer.ChangeState("Unarmed");
            return;
        }
        if(origin.switchingTo == 1)
        {
            parentLayer.ChangeState("Pistol");
            return;
        }
        if(origin.switchingTo == 2)
        {
            parentLayer.ChangeState("Knife");
            return;
        }
    }
    public override void OnStateExit()
    {
        base.OnStateExit();
        origin.useItem = false;
    }
}
