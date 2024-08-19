using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipments_Knife_Idle : PlayerEquipments_WeaponIdleState
{
    public PlayerEquipments_Knife_Idle(Player origin, Layer<Player> parent) : base(origin, parent)
    {

    }
    public override void OnStateEnter()
    {
        base.OnStateEnter();
        origin.anim.Play("Knife_Idle");
    }
    public override void OnStateUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            parentLayer.ChangeState("Slash");
            return;
        }
        if (Input.GetKeyDown(KeyCode.Alpha1) && origin.hasPistol)
        {
            origin.switchingTo = 1;
            parentLayer.ChangeState("Exit");
            return;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            origin.switchingTo = 0;
            parentLayer.ChangeState("Exit");
            return;
        }
        if (Input.GetMouseButton(1) && origin.canFocus)
        {
            parentLayer.ChangeState("Stance");
            return;
        }
        base.OnStateUpdate();
    }
}
