using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovements_Moving_Running : State<Player>
{
    public PlayerMovements_Moving_Running(Player origin, Layer<Player> parent) : base(origin, parent)
    {
        
    }
    public override void OnStateUpdate()
    {
        base.OnStateUpdate();
        origin.Stamina = Mathf.Max(0, origin.Stamina - 10.0f * Time.deltaTime);
        origin.MovePos((origin.transform.forward * Input.GetAxisRaw("Vertical") + origin.transform.right * Input.GetAxisRaw("Horizontal")).normalized * Time.deltaTime * origin.runSpeed);
        if (!Input.GetKey(KeyCode.LeftShift) || origin.Stamina <= 0.0f || !origin.canSprint)
        {
            parentLayer.ChangeState("Walking");
        }
    }
}
