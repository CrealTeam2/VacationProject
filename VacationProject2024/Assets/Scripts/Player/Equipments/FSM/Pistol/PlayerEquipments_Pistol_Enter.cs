using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipments_Pistol_Enter : PlayerEquipments_AnimationState
{
    public PlayerEquipments_Pistol_Enter(Player origin, Layer<Player> parent) : base(origin, parent, "Pistol_Enter")
    {

    }
    protected override void ClipFinish()
    {
        base.ClipFinish();
        parentLayer.ChangeState("Idle");
    }
}
