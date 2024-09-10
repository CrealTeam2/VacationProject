using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exhausted : Debuff
{
    readonly float speedMultiplier;
    public Exhausted(float speedMultiplier, float duration) : base(duration)
    {
        this.speedMultiplier = speedMultiplier;
    }
    public override void OnDebuffAdd(List<Debuff> debuffList, Player debuffed)
    {
        base.OnDebuffAdd(debuffList, debuffed);
        debuffed.speedMultiplier *= speedMultiplier;
        debuffed.breatheSFX.Stop();
        debuffed.heavyBreatheSFX.Play();
        debuffed.canSprint = false;
    }
    public override void OnDebuffEnd()
    {
        base.OnDebuffEnd();
        debuffed.speedMultiplier /= speedMultiplier;
        debuffed.breatheSFX.Play();
        debuffed.heavyBreatheSFX.Stop();
        debuffed.canSprint = true;
    }
}
