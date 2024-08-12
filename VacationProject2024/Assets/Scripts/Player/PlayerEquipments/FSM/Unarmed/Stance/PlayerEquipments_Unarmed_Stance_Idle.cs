using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipments_Unarmed_Stance_Idle : State<PlayerEquipments>
{
    public PlayerEquipments_Unarmed_Stance_Idle(PlayerEquipments origin, Layer<PlayerEquipments> parent) : base(origin, parent)
    {

    }
    public override void OnStateEnter()
    {
        base.OnStateEnter();
        origin.anim.Play("Unarmed_Stance_Idle");
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
        if(Input.GetMouseButton(1) == false)
        {
            parentLayer.ChangeState("Exit");
            return;
        }
        base.OnStateUpdate();
    }
}
