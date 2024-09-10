using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Haste : Debuff
{
    readonly float speedMultiplier;
    public Haste(float speedMultiplier, float duration) : base(duration)
    {
        this.speedMultiplier = speedMultiplier;
    }
    public override void OnDebuffAdd(List<Debuff> debuffList, Player debuffed)
    {
        base.OnDebuffAdd(debuffList, debuffed);
        debuffed.speedMultiplier *= speedMultiplier;
    }
    public override void OnUpdate()
    {
        base.OnUpdate();
        debuffed.Stamina = debuffed.maxStamina;
    }
    public override void OnDebuffEnd()
    {
        base.OnDebuffEnd();
        debuffed.speedMultiplier /= speedMultiplier;
    }
}
