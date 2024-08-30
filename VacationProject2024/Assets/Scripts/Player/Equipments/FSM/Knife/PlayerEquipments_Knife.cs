using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipments_Knife : Layer<Player>
{
    public PlayerEquipments_Knife(Player origin, Layer<Player> parent) : base(origin, parent)
    {
        defaultState = new PlayerEquipments_Knife_Enter(origin, this);
        AddState("Enter", defaultState);
        AddState("Idle", new PlayerEquipments_Knife_Idle(origin, this));
        AddState("Slash", new PlayerEquipments_Knife_Slash(origin, this));
        AddState("Stance", new PlayerEquipments_Knife_Stance(origin, this));
        AddState("Exit", new PlayerEquipments_Knife_Exit(origin, this));
    }
}
