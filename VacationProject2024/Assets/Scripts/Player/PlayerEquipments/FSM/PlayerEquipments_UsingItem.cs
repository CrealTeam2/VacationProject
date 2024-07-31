using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipments_UsingItem : State<PlayerEquipments>
{
    public PlayerEquipments_UsingItem(PlayerEquipments origin, Layer<PlayerEquipments> parent) : base(origin, parent)
    {

    }
    public override void OnStateEnter()
    {
        base.OnStateEnter();
        origin.anim.SetInteger("UsingItemNum", origin.usingItemNum);
        origin.acting = true;
    }
    public override void OnStateUpdate()
    {
        base.OnStateUpdate();
        if(origin.acting == false)
        {
            int num = origin.anim.GetInteger("ArmedState");
            if (num == 0)
            {
                parentLayer.ChangeState("Unarmed");
                return;
            }
            if(num == 1)
            {
                parentLayer.ChangeState("Pistol");
                return;
            }
            if(num == 2)
            {
                parentLayer.ChangeState("Knife");
                return;
            }
        }
    }
    public override void OnStateExit()
    {
        base.OnStateExit();
        origin.anim.SetInteger("UsingItemNum", -1);
    }
}
