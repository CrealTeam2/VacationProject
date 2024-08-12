using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipments_Unarmed_Stance_Enter : PlayerEquipments_AnimationState
{
    public PlayerEquipments_Unarmed_Stance_Enter(Player origin, Layer<Player> parent) : base(origin, parent, "Unarmed_Stance_Enter")
    {

    }
    protected override void ClipFinish()
    {
        base.ClipFinish();
        parentLayer.ChangeState("Idle");
    }
}
