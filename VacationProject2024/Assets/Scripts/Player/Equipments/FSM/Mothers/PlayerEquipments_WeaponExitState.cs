using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerEquipments_WeaponExitState : PlayerEquipments_AnimationState
{
    public PlayerEquipments_WeaponExitState(Player origin, Layer<Player> parent, string clip) : base(origin, parent, clip)
    {

    }
    protected override void ClipFinish()
    {
        base.ClipFinish();
        if (origin.useItem)
        {
            if (origin.itemNum == 0)
            {
                parentLayer.parentLayer.ChangeState("UsingBandage");
                return;
            }
            if(origin.itemNum == 1)
            {
                parentLayer.parentLayer.ChangeState("UsingMedicine");
                return;
            }
        }
    }
}
