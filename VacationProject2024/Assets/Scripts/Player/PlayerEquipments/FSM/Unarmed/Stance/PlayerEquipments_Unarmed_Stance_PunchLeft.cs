using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipments_Unarmed_Stance_PunchLeft : PlayerEquipments_AnimationState
{
    public PlayerEquipments_Unarmed_Stance_PunchLeft(PlayerEquipments origin, Layer<PlayerEquipments> parent) : base(origin, parent, "Unarmed_Stance_Punch_Left")
    {

    }
    protected override void ClipFinish()
    {
        base.ClipFinish();
        parentLayer.ChangeState("Idle");
    }
}
