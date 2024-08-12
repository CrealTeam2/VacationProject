using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipments_UsingMedicine : PlayerEquipments_ItemUseState
{
    public PlayerEquipments_UsingMedicine(Player origin, Layer<Player> parent) : base(origin, parent, "UsingMedicine")
    {

    }
}
