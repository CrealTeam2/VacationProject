using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipments_Knife_Stance_Enter : PlayerEquipments_AnimationState
{
    public PlayerEquipments_Knife_Stance_Enter(PlayerEquipments origin, Layer<PlayerEquipments> parent) : base(origin, parent, "Knife_Stance_Enter")
    {

    }
    protected override void ClipFinish()
    {
        base.ClipFinish();
        parentLayer.ChangeState("Idle");
    }
}
