using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovements_Idle : State<Player>
{
    public PlayerMovements_Idle(Player origin, Layer<Player> parent) : base(origin, parent)
    {
        
    }
    public override void OnStateEnter()
    {
        base.OnStateEnter();
        origin.anim.Play("Idle", 1);
    }
    public override void OnStateUpdate()
    {
        base.OnStateUpdate();
        origin.Stamina = Mathf.Min(origin.maxStamina, origin.Stamina + 10.0f * Time.deltaTime);
        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            parentLayer.ChangeState("Moving");
        }
    }
}
