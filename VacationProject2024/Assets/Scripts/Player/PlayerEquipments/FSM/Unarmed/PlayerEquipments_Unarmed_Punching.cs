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
        origin.anim.SetTrigger("Attack");
        origin.acting = true;
        origin.canSwap = false;
    }
    public override void OnStateUpdate()
    {
        base.OnStateUpdate();
        if(origin.acting == false)
        {
            parentLayer.ChangeState("Idle");
        }
    }
    public override void OnStateExit()
    {
        base.OnStateExit();
        origin.canSwap = true;
    }
}
