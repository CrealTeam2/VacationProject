using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipments_Unarmed_PunchLeft : PlayerEquipments_AnimationState
{
    public PlayerEquipments_Unarmed_PunchLeft(Player origin, Layer<Player> parent) : base(origin, parent, "Unarmed_Punch_Left")
    {

    }
    public override void OnStateEnter()
    {
        base.OnStateEnter();
        origin.leftFistHitbox.enabled = true;
        origin.leftFistHitbox.onHit += origin.FistHit;
    }
    protected override void ClipFinish()
    {
        base.ClipFinish();
        parentLayer.ChangeState("Idle");
    }
    public override void OnStateExit()
    {
        base.OnStateExit();
        origin.leftFistHitbox.enabled = false;
        origin.leftFistHitbox.onHit -= origin.FistHit;
    }
}
