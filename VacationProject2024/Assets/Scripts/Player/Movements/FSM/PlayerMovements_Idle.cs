using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovements_Idle : State<Player>
{
    public PlayerMovements_Idle(Player origin, Layer<Player> parent) : base(origin, parent)
    {
        
    }
    public override void OnStateFixedUpdate()
    {
        base.OnStateFixedUpdate();
        origin.Stamina = Mathf.Min(origin.maxStamina, origin.Stamina + 10.0f * Time.fixedDeltaTime);
        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            parentLayer.ChangeState("Moving");
        }
    }
}
