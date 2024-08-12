using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipments_Unarmed_PunchLeft : PlayerEquipments_AnimationState
{
    public PlayerEquipments_Unarmed_PunchLeft(Player origin, Layer<Player> parent) : base(origin, parent, "Unarmed_Punch_Left")
    {

    }
    protected override void ClipFinish()
    {
        base.ClipFinish();
        parentLayer.ChangeState("Idle");
    }
}
