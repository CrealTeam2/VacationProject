using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipments_Unarmed : Layer<PlayerEquipments>
{
    public PlayerEquipments_Unarmed(PlayerEquipments origin, Layer<PlayerEquipments> parent) : base(origin, parent)
    {
        defaultState = new PlayerEquipments_Unarmed_Enter(origin, this);
        AddState("Enter", defaultState);
        AddState("Idle", new PlayerEquipments_Unarmed_Idle(origin, this));
        AddState("Punching_Right", new PlayerEquipments_Unarmed_PunchRight(origin, this));
        AddState("Punching_Left", new PlayerEquipments_Unarmed_PunchLeft(origin, this));
        AddState("Stance", new PlayerEquipments_Unarmed_Stance(origin, this));
        AddState("Exit", new PlayerEquipments_Unarmed_Exit(origin, this));
    }
}
