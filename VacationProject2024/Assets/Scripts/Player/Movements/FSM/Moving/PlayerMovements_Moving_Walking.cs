using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovements_Moving_Walking : State<Player>
{
    public PlayerMovements_Moving_Walking(Player origin, Layer<Player> parent) : base(origin, parent)
    {
        
    }
    public override void OnStateFixedUpdate()
    {
        base.OnStateFixedUpdate();
        origin.Stamina = Mathf.Min(origin.maxStamina, origin.Stamina + 5.0f * Time.fixedDeltaTime);
        origin.MovePos((origin.transform.forward * Input.GetAxisRaw("Vertical") + origin.transform.right * Input.GetAxisRaw("Horizontal")).normalized * Time.fixedDeltaTime * origin.walkSpeed);
        if(Input.GetKey(KeyCode.LeftShift) && origin.Stamina > 10.0f && origin.canSprint)
        {
            parentLayer.ChangeState("Running");
        }
    }
}
