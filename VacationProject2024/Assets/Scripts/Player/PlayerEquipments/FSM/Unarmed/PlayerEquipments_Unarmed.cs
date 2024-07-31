using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipments_Unarmed : Layer<PlayerEquipments>
{
    public PlayerEquipments_Unarmed(PlayerEquipments origin, Layer<PlayerEquipments> parent) : base(origin, parent)
    {
        defaultState = new PlayerEquipments_AnimationState(origin, this, "Unarmed_Enter", "Idle");
        AddState("Entering", defaultState);
        AddState("Idle", new PlayerEquipments_Unarmed_Idle(origin, this));
        AddState("Punching", new PlayerEquipments_AnimationState(origin, this, "Unarmed_Punch", "Idle"));
    }
}
