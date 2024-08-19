using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipments_Knife_Stance : Layer<Player>
{
    public PlayerEquipments_Knife_Stance(Player origin, Layer<Player> parent) : base(origin, parent)
    {
        defaultState = new PlayerEquipments_Knife_Stance_Enter(origin, this);
        AddState("Enter", defaultState);
        AddState("Idle", new PlayerEquipments_Knife_Stance_Idle(origin, this));
        AddState("Slash", new PlayerEquipments_Knife_Stance_Slash(origin, this));
        AddState("Exit", new PlayerEquipments_Knife_Stance_Exit(origin, this));
    }
    public override void OnStateEnter()
    {
        base.OnStateEnter();
        origin.speedMultiplier *= Player.focusSlowScale;
        origin.canSprint = false;
    }
    public override void OnStateUpdate()
    {
        base.OnStateUpdate();
        if (!origin.canFocus) ChangeState("Exit");
    }
    public override void OnStateExit()
    {
        base.OnStateExit();
        origin.speedMultiplier /= Player.focusSlowScale;
    }
}
