using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotting : Debuff
{
    const float slowScale = 0.9f;
    const float healthLoss = 0.5f, healthLossTick = 1.0f;
    public Rotting() : base(Mathf.Infinity)
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
        debuffed.onMedicineUse += EndDebuff;
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
        debuffed.onMedicineUse -= EndDebuff;
    }
}
