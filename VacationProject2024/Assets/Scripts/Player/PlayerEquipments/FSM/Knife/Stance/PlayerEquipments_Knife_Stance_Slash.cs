using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipments_Knife_Stance_Slash : PlayerEquipments_AnimationState
{
    public PlayerEquipments_Knife_Stance_Slash(PlayerEquipments origin, Layer<PlayerEquipments> parent) : base(origin, parent, "Knife_Stance_Slash")
    {

    }
    protected override void ClipFinish()
    {
        base.ClipFinish();
        parentLayer.ChangeState("Idle");
    }
}
