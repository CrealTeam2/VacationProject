using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipments_Knife_Stance : Layer<PlayerEquipments>
{
    public PlayerEquipments_Knife_Stance(PlayerEquipments origin, Layer<PlayerEquipments> parent) : base(origin, parent)
    {
        defaultState = new PlayerEquipments_Knife_Stance_Enter(origin, this);
        AddState("Enter", defaultState);
        AddState("Idle", new PlayerEquipments_Knife_Stance_Idle(origin, this));
        AddState("Slash", new PlayerEquipments_Knife_Stance_Slash(origin, this));
        AddState("Exit", new PlayerEquipments_Knife_Stance_Exit(origin, this));
    }
}
