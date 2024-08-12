using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipments_Unarmed_Stance_Exit : PlayerEquipments_AnimationState
{
    public PlayerEquipments_Unarmed_Stance_Exit(PlayerEquipments origin, Layer<PlayerEquipments> parent) : base(origin, parent, "Unarmed_Stance_Exit")
    {

    }
    protected override void ClipFinish()
    {
        base.ClipFinish();
        parentLayer.parentLayer.ChangeState("Idle");
    }
}
