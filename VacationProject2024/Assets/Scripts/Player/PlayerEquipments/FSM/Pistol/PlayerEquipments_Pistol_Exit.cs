using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipments_Pistol_Exit : PlayerEquipments_AnimationState
{
    public PlayerEquipments_Pistol_Exit(PlayerEquipments origin, Layer<PlayerEquipments> parent) : base(origin, parent, "Pistol_Exit")
    {

    }
    protected override void ClipFinish()
    {
        base.ClipFinish();
        if (origin.switchingTo == 0)
        {
            parentLayer.parentLayer.ChangeState("Unarmed");
            return;
        }
        if (origin.switchingTo == 2)
        {
            parentLayer.parentLayer.ChangeState("Knife");
            return;
        }
    }
}
