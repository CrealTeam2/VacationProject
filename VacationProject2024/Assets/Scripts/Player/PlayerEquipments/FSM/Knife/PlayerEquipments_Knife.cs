using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipments_Knife : Layer<PlayerEquipments>
{
    public PlayerEquipments_Knife(PlayerEquipments origin, Layer<PlayerEquipments> parent) : base(origin, parent)
    {
        defaultState = new PlayerEquipments_Knife_Enter(origin, this);
        AddState("Enter", defaultState);
        AddState("Idle", new PlayerEquipments_Knife_Idle(origin, this));
        AddState("Slash", new PlayerEquipments_Knife_Slash(origin, this));
        AddState("Exit", new PlayerEquipments_Knife_Exit(origin, this));
    }
    public override void OnStateEnter()
    {
        base.OnStateEnter();
        origin.anim.SetInteger("ArmedState", 2);
    }
}
