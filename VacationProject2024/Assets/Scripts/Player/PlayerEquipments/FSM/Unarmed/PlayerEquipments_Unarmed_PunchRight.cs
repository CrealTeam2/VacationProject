using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipments_Unarmed_PunchRight : PlayerEquipments_AnimationState
{
    public PlayerEquipments_Unarmed_PunchRight(PlayerEquipments origin, Layer<PlayerEquipments> parent) : base(origin, parent, "Unarmed_Punch_Right")
    {

    }
    protected override void ClipFinish()
    {
        base.ClipFinish();
        parentLayer.ChangeState("Idle");
    }
}
