using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovements_Moving : Layer<Player>
{
    public PlayerMovements_Moving(Player origin, Layer<Player> parent) : base(origin, parent)
    {
        AddState("Walking", new PlayerMovements_Moving_Walking(origin, this));
        AddState("Running", new PlayerMovements_Moving_Running(origin, this));
    }
    public override void OnStateEnter()
    {
        if(Input.GetKey(KeyCode.LeftShift) && origin.Stamina > 10.0f)
        {
            currentState = states["Running"];
        }
        else
        {
            currentState = states["Walking"];
        }
        currentState.OnStateEnter();
        origin.anim.SetBool("Moving", true);
    }
    public override void OnStateUpdate()
    {
        base.OnStateUpdate();
        origin.anim.SetFloat("MoveX", Input.GetAxis("Horizontal"));
        origin.anim.SetFloat("MoveY", Input.GetAxis("Vertical"));
        if(Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0)
        {
            parentLayer.ChangeState("Idle");
        }
    }
    public override void OnStateExit()
    {
        base.OnStateExit();
        origin.anim.SetBool("Moving", false);
    }
}
