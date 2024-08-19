using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Injured : Debuff
{
    const float slowScale = 0.7f;
    const float healthLoss = 0.5f, healthLossTick = 1.0f;
    public Injured(float duration) : base(duration)
    {

    }
    public override void OnDebuffAdd(List<Debuff> debuffList, Player debuffed)
    {
        foreach(var i in debuffList)
        {
            if(i is Injured)
            {
                i.ResetDuration();
                return;
            }
        }
        base.OnDebuffAdd(debuffList, debuffed);
        debuffed.speedMultiplier *= slowScale;
        debuffed.canSprint = false;
        debuffed.canFocus = false;
        debuffed.onBandageUse += EndDebuff;
    }
    float counter = 0.0f;
    public override void OnUpdate()
    {
        base.OnUpdate();
        if (counter < healthLossTick) counter += Time.deltaTime;
        else
        {
            counter -= healthLossTick;
            debuffed.GetDamage(healthLoss);
        }
    }
    public override void OnDebuffEnd()
    {
        base.OnDebuffEnd();
        debuffed.speedMultiplier /= slowScale;
        debuffed.canSprint = true;
        debuffed.canFocus = true;
        debuffed.onBandageUse -= EndDebuff;
    }
}
