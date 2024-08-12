using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipments_Pistol_Reloading : PlayerEquipments_AnimationState
{
    public PlayerEquipments_Pistol_Reloading(PlayerEquipments origin, Layer<PlayerEquipments> parent) : base(origin, parent, "Pistol_Reload")
    {

    }
    protected override void ClipFinish()
    {
        base.ClipFinish();
        int tmp = origin.pistolMag;
        origin.pistolMag = Mathf.Min(origin.pistolMagSize, origin.pistolMag + origin.bullets);
        origin.bullets -= origin.pistolMag - tmp;
        parentLayer.ChangeState("Idle");
    }
}
