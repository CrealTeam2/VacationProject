using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipments_TopLayer : TopLayer<PlayerEquipments>
{
    public PlayerEquipments_TopLayer(PlayerEquipments origin) : base(origin)
    {
        defaultState = new PlayerEquipments_Unarmed(origin, this);
        AddState("Unarmed", defaultState);
        AddState("Pistol", new PlayerEquipments_Pistol(origin, this));
        AddState("Knife", new PlayerEquipments_Knife(origin, this));
        AddState("UsingItem", new PlayerEquipments_UsingItem(origin, this));
    }
}
