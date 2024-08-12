using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipments_Pistol_Aiming_Enter : PlayerEquipments_AnimationState
{
    public PlayerEquipments_Pistol_Aiming_Enter(Player origin, Layer<Player> parent) : base(origin, parent, "Pistol_Aiming_Enter")
    {

    }
    protected override void ClipFinish()
    {
        base.ClipFinish();
        parentLayer.ChangeState("Idle");
    }
}
