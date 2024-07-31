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
        for(int i = 0; i < 3; i++)
        {
            if(Input.GetKeyDown(KeyCode.Alpha3 + i))
            {
                origin.usingItemNum = i;
                parentLayer.parentLayer.ChangeState("UsingItem");
                return;
            }
        }
    }
}
