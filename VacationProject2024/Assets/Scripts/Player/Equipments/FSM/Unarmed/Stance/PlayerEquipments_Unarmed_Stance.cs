using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipments_Unarmed_Stance : Layer<Player>
{
    public PlayerEquipments_Unarmed_Stance(Player origin, Layer<Player> parent) : base(origin, parent)
    {
        defaultState = new PlayerEquipments_Unarmed_Stance_Enter(origin, this);
        AddState("Enter", defaultState);
        AddState("Idle", new PlayerEquipments_Unarmed_Stance_Idle(origin, this));
        AddState("Punching_Right", new PlayerEquipments_Unarmed_Stance_PunchRight(origin, this));
        AddState("Punching_Left", new PlayerEquipments_Unarmed_Stance_PunchLeft(origin, this));
        AddState("Exit", new PlayerEquipments_Unarmed_Stance_Exit(origin, this));
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
