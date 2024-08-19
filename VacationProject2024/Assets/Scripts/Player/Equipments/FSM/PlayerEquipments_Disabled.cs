using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipments_Disabled : State<Player>
{
    public PlayerEquipments_Disabled(Player origin, Layer<Player> parent) : base(origin, parent)
    {
        
    }
    public override void OnStateEnter()
    {
        base.OnStateEnter();
        origin.anim.Play("Disabled");
    }
}
