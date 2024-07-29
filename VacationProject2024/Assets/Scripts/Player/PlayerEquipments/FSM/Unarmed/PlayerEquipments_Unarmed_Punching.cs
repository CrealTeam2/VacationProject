using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipments_Unarmed_Punching : State<PlayerEquipments>
{
    public PlayerEquipments_Unarmed_Punching(PlayerEquipments origin, Layer<PlayerEquipments> parent) : base(origin, parent)
    {

    }
    public override void OnStateEnter()
    {
        base.OnStateEnter();
        origin.anim.SetBool("PunchRight", !origin.anim.GetBool("PunchRight"));
        origin.anim.SetTrigger("Attack");
        origin.acting = true;
    }
    public override void OnStateUpdate()
    {
        base.OnStateUpdate();
        if(origin.acting == false)
        {
            parentLayer.ChangeState("Idle");
        }
    }
}
