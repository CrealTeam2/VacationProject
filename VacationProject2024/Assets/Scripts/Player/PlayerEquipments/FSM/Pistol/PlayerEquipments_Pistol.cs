using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipments_Pistol : Layer<PlayerEquipments>
{
    public PlayerEquipments_Pistol(PlayerEquipments origin, Layer<PlayerEquipments> parent) : base(origin, parent)
    {
        defaultState = new PlayerEquipments_Pistol_Idle(origin, this);
        AddState("Idle", defaultState);
        AddState("Reloading", new PlayerEquipments_Pistol_Reloading(origin, this));
    }
    public override void OnStateEnter()
    {
        base.OnStateEnter();
        origin.anim.SetInteger("ArmedState", 1);
    }
    public override void OnStateExit()
    {
        base.OnStateExit();
        origin.switching = true;
    }
}
