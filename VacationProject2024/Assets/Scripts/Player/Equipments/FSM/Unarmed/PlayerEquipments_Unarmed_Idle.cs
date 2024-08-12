using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipments_Unarmed_Idle : PlayerEquipments_WeaponIdleState
{
    public PlayerEquipments_Unarmed_Idle(Player origin, Layer<Player> parent) : base(origin, parent)
    {

    }
    public override void OnStateEnter()
    {
        base.OnStateEnter();
        origin.anim.Play("Unarmed_Idle");
    }
    public override void OnStateUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (origin.punchedRight)
            {
                origin.punchedRight = false;
                parentLayer.ChangeState("Punching_Left");
                return;
            }
            else
            {
                origin.punchedRight = true;
                parentLayer.ChangeState("Punching_Right");
                return;
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha1) && origin.hasPistol)
        {
            origin.switchingTo = 1;
            parentLayer.ChangeState("Exit");
            return;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && origin.hasKnife)
        {
            origin.switchingTo = 2;
            parentLayer.ChangeState("Exit");
            return;
        }
        if (Input.GetMouseButton(1) == true)
        {
            parentLayer.ChangeState("Stance");
            return;
        }
        base.OnStateUpdate();
    }
}
