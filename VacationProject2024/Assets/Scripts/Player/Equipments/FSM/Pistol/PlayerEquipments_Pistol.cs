using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipments_Pistol : Layer<Player>
{
    public PlayerEquipments_Pistol(Player origin, Layer<Player> parent) : base(origin, parent)
    {
        defaultState = new PlayerEquipments_Pistol_Enter(origin, this);
        AddState("Enter", defaultState);
        AddState("Idle", new PlayerEquipments_Pistol_Idle(origin, this));
        AddState("Reloading", new PlayerEquipments_Pistol_Reloading(origin, this));
        AddState("Aiming", new PlayerEquipments_Pistol_Aiming(origin, this));
        AddState("Exit", new PlayerEquipments_Pistol_Exit(origin, this));
    }
    public override void OnStateEnter()
    {
        base.OnStateEnter();
        origin.anim.SetInteger("ArmedState", 1);
    }
}
