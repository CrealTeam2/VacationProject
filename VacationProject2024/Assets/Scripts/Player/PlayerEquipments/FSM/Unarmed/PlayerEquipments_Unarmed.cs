using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipments_Unarmed : Layer<PlayerEquipments>
{
    public PlayerEquipments_Unarmed(PlayerEquipments origin, Layer<PlayerEquipments> parent) : base(origin, parent)
    {
        defaultState = new PlayerEquipments_Unarmed_Idle(origin, this);
        AddState("Idle", defaultState);
        AddState("Punching", new PlayerEquipments_Unarmed_Punching(origin, this));
    }
    public override void OnStateEnter()
    {
        base.OnStateEnter();
        origin.anim.SetInteger("ArmedState", 0);
    }
}
