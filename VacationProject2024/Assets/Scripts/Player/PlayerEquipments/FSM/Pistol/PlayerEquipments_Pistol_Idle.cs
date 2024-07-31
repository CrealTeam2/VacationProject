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
        counter = 0.0f;
    }
    public override void OnStateUpdate()
    {
        if (counter < origin.pistolFireRate) counter += Time.deltaTime;
        else
        {
            if (Input.GetMouseButton(0) && origin.pistolMag > 0)
            {
                origin.anim.SetTrigger("Attack");
                counter -= origin.pistolFireRate;
                origin.pistolMag--;
                Fire();
            }
            else if(Input.GetKeyDown(KeyCode.R) && origin.pistolMag < origin.pistolMagSize && origin.bullets > 0)
            {
                parentLayer.ChangeState("Reloading");
                return;
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    parentLayer.parentLayer.ChangeState("Unarmed");
                    return;
                }
                else if (Input.GetKeyDown(KeyCode.Alpha2) && origin.hasKnife)
                {
                    parentLayer.parentLayer.ChangeState("Knife");
                    return;
                }
            }
        }
        base.OnStateUpdate();
    }
    void Fire()
    {
        Debug.Log("fired");
    }
}
