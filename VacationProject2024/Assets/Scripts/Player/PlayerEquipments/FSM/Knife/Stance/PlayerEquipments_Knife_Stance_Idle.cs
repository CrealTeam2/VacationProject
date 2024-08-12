using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipments_Knife_Stance_Idle : State<PlayerEquipments>
{
    public PlayerEquipments_Knife_Stance_Idle(PlayerEquipments origin, Layer<PlayerEquipments> parent) : base(origin, parent)
    {

    }
    public override void OnStateEnter()
    {
        base.OnStateEnter();
        origin.anim.Play("Knife_Stance_Idle");
    }
    public override void OnStateUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            parentLayer.ChangeState("Slash");
            return;
        }
        if(Input.GetMouseButton(1) == false)
        {
            parentLayer.ChangeState("Exit");
            return;
        }
        base.OnStateUpdate();
    }
}
