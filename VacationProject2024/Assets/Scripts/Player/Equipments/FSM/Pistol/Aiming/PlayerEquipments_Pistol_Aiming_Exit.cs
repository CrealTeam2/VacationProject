using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipments_Pistol_Aiming_Exit : PlayerEquipments_AnimationState
{
    public PlayerEquipments_Pistol_Aiming_Exit(Player origin, Layer<Player> parent) : base(origin, parent, "Pistol_Aiming_Exit")
    {

    }
    bool reloadQueued = false;
    public override void OnStateEnter()
    {
        base.OnStateEnter();
        origin.canSprint = true;
    }
    public override void OnStateUpdate()
    {
        base.OnStateUpdate();
        if (Input.GetKeyDown(KeyCode.R))
        {
            origin.reloadQueued = true;
        }
    }
    protected override void ClipFinish()
    {
        base.ClipFinish();
        if (origin.reloadQueued)
        {
            parentLayer.parentLayer.ChangeState("Reloading");
            return;
        }
        parentLayer.parentLayer.ChangeState("Idle");
    }
}
