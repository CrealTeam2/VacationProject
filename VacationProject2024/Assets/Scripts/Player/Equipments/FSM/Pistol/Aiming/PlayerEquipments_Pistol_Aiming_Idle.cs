using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipments_Pistol_Aiming_Idle : State<Player>
{
    public PlayerEquipments_Pistol_Aiming_Idle(Player origin, Layer<Player> parent) : base(origin, parent)
    {

    }
    public override void OnStateEnter()
    {
        base.OnStateEnter();
        origin.reloadQueued = false;
        origin.anim.Play("Pistol_Aiming_Idle");
        origin.crosshair.SetActive(true);
    }
    public override void OnStateUpdate()
    {
        base.OnStateUpdate();
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
        if(Input.GetMouseButton(1) == false)
        {
            parentLayer.ChangeState("Exit");
            return;
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            origin.reloadQueued = true;
            parentLayer.ChangeState("Exit");
            return;
        }
    }
    void Fire()
    {
        origin.anim.SetTrigger("Fire");
        RaycastHit hit;
        if (Physics.Raycast(origin.firePoint.position, origin.firePoint.forward, out hit, Mathf.Infinity, LayerMask.GetMask("Enemy")))
        {
            hit.transform.GetComponent<EnemyTest>()?.GetDamage(origin.pistolDamage);
        }
    }
    public override void OnStateExit()
    {
        base.OnStateExit();
        origin.crosshair.SetActive(false);
    }
}
