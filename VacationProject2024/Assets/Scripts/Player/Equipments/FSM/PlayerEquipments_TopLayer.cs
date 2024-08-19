using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipments_TopLayer : TopLayer<Player>
{
    public PlayerEquipments_TopLayer(Player origin) : base(origin)
    {
        defaultState = new PlayerEquipments_Unarmed(origin, this);
        AddState("Unarmed", defaultState);
        AddState("Pistol", new PlayerEquipments_Pistol(origin, this));
        AddState("Knife", new PlayerEquipments_Knife(origin, this));
        AddState("UsingBandage", new PlayerEquipments_UsingBandage(origin, this));
        AddState("UsingMedicine", new PlayerEquipments_UsingMedicine(origin, this));
        AddState("Disabled", new PlayerEquipments_Disabled(origin, this));
    }
}
