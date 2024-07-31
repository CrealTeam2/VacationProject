using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipments_Knife : Layer<PlayerEquipments>
{
    public PlayerEquipments_Knife(PlayerEquipments origin, Layer<PlayerEquipments> parent) : base(origin, parent)
    {

    }
    public override void OnStateEnter()
    {
        base.OnStateEnter();
        origin.anim.SetInteger("ArmedState", 2);
    }
    public override void OnStateExit()
    {
        base.OnStateExit();
        origin.switching = true;
    }
}
