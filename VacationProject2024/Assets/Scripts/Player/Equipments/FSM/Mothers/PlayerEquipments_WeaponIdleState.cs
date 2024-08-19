using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipments_WeaponIdleState : State<Player>
{
    public PlayerEquipments_WeaponIdleState(Player origin, Layer<Player> parent) : base(origin, parent)
    {

    }
    public override void OnStateUpdate()
    {
        base.OnStateUpdate();
        if (Input.GetKeyDown(KeyCode.E) && origin.bandages > 0)
        {
            parentLayer.ChangeState("Exit");
            origin.useItem = true;
            origin.itemNum = 0;
            return;
        }
        if (Input.GetKeyDown(KeyCode.Q) && origin.medicines > 0)
        {
            parentLayer.ChangeState("Exit");
            origin.useItem = true;
            origin.itemNum = 1;
            return;
        }
    }
}
