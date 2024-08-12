using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipments_WeaponIdleState : State<PlayerEquipments>
{
    public PlayerEquipments_WeaponIdleState(PlayerEquipments origin, Layer<PlayerEquipments> parent) : base(origin, parent)
    {

    }
    public override void OnStateUpdate()
    {
        base.OnStateUpdate();
        if (Input.GetKeyDown(KeyCode.E))
        {
            parentLayer.ChangeState("Exit");
            origin.useItem = true;
            origin.itemNum = 0;
            return;
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            parentLayer.ChangeState("Exit");
            origin.useItem = true;
            origin.itemNum = 1;
            return;
        }
    }
}
