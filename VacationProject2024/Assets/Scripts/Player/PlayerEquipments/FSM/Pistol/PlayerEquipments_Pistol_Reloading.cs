using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipments_Pistol_Reloading : State<PlayerEquipments>
{
    public PlayerEquipments_Pistol_Reloading(PlayerEquipments origin, Layer<PlayerEquipments> parent) : base(origin, parent)
    {

    }
    public override void OnStateEnter()
    {
        base.OnStateEnter();
        origin.acting = true;
        origin.anim.SetTrigger("PistolReload");
    }
    public override void OnStateUpdate()
    {
        base.OnStateUpdate();
        if(origin.acting == false)
        {
            int tmp = origin.pistolMag;
            origin.pistolMag = Mathf.Min(origin.pistolMagSize, origin.pistolMag + origin.bullets);
            origin.bullets -= origin.pistolMag - tmp;
            parentLayer.ChangeState("Idle");
        }
    }
}
