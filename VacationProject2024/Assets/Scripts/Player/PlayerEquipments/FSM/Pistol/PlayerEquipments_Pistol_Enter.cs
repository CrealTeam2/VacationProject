using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipments_Pistol_Enter : PlayerEquipments_AnimationState
{
    public PlayerEquipments_Pistol_Enter(PlayerEquipments origin, Layer<PlayerEquipments> parent) : base(origin, parent, "Pistol_Enter")
    {

    }
    protected override void ClipFinish()
    {
        base.ClipFinish();
        parentLayer.ChangeState("Idle");
    }
}
