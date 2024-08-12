using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipments_Pistol_Exit : PlayerEquipments_WeaponExitState
{
    public PlayerEquipments_Pistol_Exit(Player origin, Layer<Player> parent) : base(origin, parent, "Pistol_Exit")
    {

    }
    protected override void ClipFinish()
    {
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
        base.ClipFinish();
    }
}
