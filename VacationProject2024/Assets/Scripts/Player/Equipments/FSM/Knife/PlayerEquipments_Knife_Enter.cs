using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipments_Knife_Enter : PlayerEquipments_AnimationState
{
    public PlayerEquipments_Knife_Enter(Player origin, Layer<Player> parent) : base(origin, parent, "Knife_Enter")
    {

    }
    protected override void ClipFinish()
    {
        base.ClipFinish();
        parentLayer.ChangeState("Idle");
    }
}
