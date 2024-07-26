using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipments_Unarmed_Idle : State<PlayerEquipments>
{
    public PlayerEquipments_Unarmed_Idle(PlayerEquipments origin, Layer<PlayerEquipments> parent) : base(origin, parent)
    {

    }
    public override void OnStateUpdate()
    {
        base.OnStateUpdate();
        Debug.Log("EAWE");
        if (Input.GetMouseButtonDown(0))
        {
            parentLayer.ChangeState("Punching");
        }
    }
}
