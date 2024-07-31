using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipments_Pistol_Idle : PlayerEquipments_WeaponIdleState
{
    public PlayerEquipments_Pistol_Idle(PlayerEquipments origin, Layer<PlayerEquipments> parent) : base(origin, parent)
    {

    }
    float counter = 0.0f;
    public override void OnStateEnter()
    {
        base.OnStateEnter();
        origin.anim.Play("Pistol_Idle");
        counter = 0.0f;
    }
    public override void OnStateUpdate()
    {
        if (counter < origin.pistolFireRate) counter += Time.deltaTime;
        else
        {
            if (Input.GetMouseButton(0) && origin.pistolMag > 0)
            {
                counter -= origin.pistolFireRate;
                origin.pistolMag--;
                Fire();
            }
        }
        if (Input.GetKeyDown(KeyCode.R) && origin.pistolMag < origin.pistolMagSize && origin.bullets > 0)
        {
            parentLayer.ChangeState("Reloading");
            return;
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            origin.switchingTo = 0;
            parentLayer.ChangeState("Exit");
            return;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && origin.hasKnife)
        {
            origin.switchingTo = 2;
            parentLayer.ChangeState("Exit");
            return;
        }
        base.OnStateUpdate();
    }
    void Fire()
    {
        origin.anim.Play("Pistol_Fire");
    }
}
