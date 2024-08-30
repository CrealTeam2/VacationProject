using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipments_Knife_Slash : PlayerEquipments_AnimationState
{
    public PlayerEquipments_Knife_Slash(Player origin, Layer<Player> parent) : base(origin, parent, "Knife_Slash")
    {

    }
    public override void OnStateEnter()
    {
        base.OnStateEnter();
        origin.knifeHitbox.onHit += origin.KnifeHit;
    }
    protected override void ClipFinish()
    {
        base.ClipFinish();
        parentLayer.ChangeState("Idle");
    }
    public override void OnStateExit()
    {
        base.OnStateExit();
        origin.knifeHitbox.onHit -= origin.KnifeHit;
    }
}
