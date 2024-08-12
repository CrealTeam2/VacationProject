using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipments_Pistol_Idle : PlayerEquipments_WeaponIdleState
{
    public PlayerEquipments_Pistol_Idle(PlayerEquipments origin, Layer<PlayerEquipments> parent) : base(origin, parent)
    {

    }
    public override void OnStateEnter()
    {
        base.OnStateEnter();
        origin.anim.Play("Pistol_Idle");
        origin.pistolCounter = 0.0f;
    }
    public override void OnStateUpdate()
    {
        if (origin.pistolCounter < origin.pistolFireRate) origin.pistolCounter += Time.deltaTime;
        else
        {
            if (Input.GetMouseButtonDown(0) && origin.pistolMag > 0)
            {
                origin.pistolCounter -= origin.pistolFireRate;
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
        if(Input.GetMouseButton(1) == true)
        {
            parentLayer.ChangeState("Aiming");
            return;
        }
        base.OnStateUpdate();
    }
    void Fire()
    {
        origin.anim.SetTrigger("Fire");
        RaycastHit hit;
        if (Physics.Raycast(origin.firePoint.position, origin.firePoint.forward, out hit, Mathf.Infinity, LayerMask.GetMask("Enemy")))
        {
            hit.transform.GetComponent<EnemyTest>().GetDamage(origin.pistolDamage);
        }
    }
}
