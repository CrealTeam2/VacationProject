using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipments_Unarmed_Exit : PlayerEquipments_WeaponExitState
{
    public PlayerEquipments_Unarmed_Exit(PlayerEquipments origin, Layer<PlayerEquipments> parent) : base(origin, parent, "Unarmed_Exit")
    {

    }
    protected override void ClipFinish()
    {
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
        base.ClipFinish();
    }
}
