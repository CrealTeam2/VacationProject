using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipments_Pistol_Aiming : Layer<Player>
{
    public PlayerEquipments_Pistol_Aiming(Player origin, Layer<Player> parent) : base(origin, parent)
    {
        defaultState = new PlayerEquipments_Pistol_Aiming_Enter(origin, this);
        AddState("Enter", defaultState);
        AddState("Idle", new PlayerEquipments_Pistol_Aiming_Idle(origin, this));
        AddState("Exit", new PlayerEquipments_Pistol_Aiming_Exit(origin, this));
    }
}
