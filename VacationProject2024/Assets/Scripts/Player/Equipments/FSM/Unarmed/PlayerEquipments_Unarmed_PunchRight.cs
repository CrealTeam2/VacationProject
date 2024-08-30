using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipments_Unarmed_PunchRight : PlayerEquipments_AnimationState
{
    public PlayerEquipments_Unarmed_PunchRight(Player origin, Layer<Player> parent) : base(origin, parent, "Unarmed_Punch_Right")
    {

    }
    public override void OnStateEnter()
    {
        base.OnStateEnter();
        origin.rightFistHitbox.enabled = true;
        origin.rightFistHitbox.onHit += origin.FistHit;
    }
    protected override void ClipFinish()
    {
        base.ClipFinish();
        parentLayer.ChangeState("Idle");
    }
    public override void OnStateExit()
    {
        base.OnStateExit();
        origin.rightFistHitbox.enabled = false;
        origin.rightFistHitbox.onHit -= origin.FistHit;
    }
}
