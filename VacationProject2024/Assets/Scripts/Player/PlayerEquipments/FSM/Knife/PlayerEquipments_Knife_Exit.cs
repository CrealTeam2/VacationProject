using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipments_Knife_Exit : PlayerEquipments_WeaponExitState
{
    public PlayerEquipments_Knife_Exit(PlayerEquipments origin, Layer<PlayerEquipments> parent) : base(origin, parent, "Knife_Exit")
    {

    }
    protected override void ClipFinish()
    {
        if(origin.switchingTo == 0)
        {
            parentLayer.parentLayer.ChangeState("Unarmed");
            return;
        }
        if (origin.switchingTo == 1)
        {
            parentLayer.parentLayer.ChangeState("Pistol");
            return;
        }
        base.ClipFinish();
    }
}
