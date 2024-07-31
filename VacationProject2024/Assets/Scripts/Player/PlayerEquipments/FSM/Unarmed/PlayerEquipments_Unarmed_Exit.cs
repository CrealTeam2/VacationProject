using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipments_Unarmed_Exit : PlayerEquipments_AnimationState
{
    public PlayerEquipments_Unarmed_Exit(PlayerEquipments origin, Layer<PlayerEquipments> parent) : base(origin, parent, "Unarmed_Exit")
    {

    }
    protected override void ClipFinish()
    {
        base.ClipFinish();
        if(origin.switchingTo == 1)
        {
            parentLayer.parentLayer.ChangeState("Pistol");
            return;
        }
        if (origin.switchingTo == 2)
        {
            parentLayer.parentLayer.ChangeState("Knife");
            return;
        }
    }
}
