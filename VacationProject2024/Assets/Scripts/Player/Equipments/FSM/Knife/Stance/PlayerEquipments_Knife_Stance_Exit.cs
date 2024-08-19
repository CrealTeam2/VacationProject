using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipments_Knife_Stance_Exit : PlayerEquipments_AnimationState
{
    public PlayerEquipments_Knife_Stance_Exit(Player origin, Layer<Player> parent) : base(origin, parent, "Knife_Stance_Exit")
    {

    }
    public override void OnStateEnter()
    {
        base.OnStateEnter();
        origin.canSprint = true;
    }
    protected override void ClipFinish()
    {
        base.ClipFinish();
        parentLayer.parentLayer.ChangeState("Idle");
    }
}
