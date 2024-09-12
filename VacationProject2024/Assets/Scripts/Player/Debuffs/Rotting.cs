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
        debuffed.AddVignetteQueue(new RottingVignetteQueue(this));
        debuffed.Talk("몸에서 썩는 듯한 냄새가 난다...");
    }
    float counter = 0.0f;
    public override void OnUpdate()
    {
        base.OnUpdate();
        if (counter < healthLossTick) counter += Time.deltaTime;
        else
        {
            counter -= healthLossTick;
            debuffed.GetDamage(healthLoss, false);
        }
    }
    public override void OnDebuffEnd()
    {
        base.OnDebuffEnd();
        debuffed.speedMultiplier /= slowScale;
        debuffed.canSprint = true;
        debuffed.onMedicineUse -= EndDebuff;
    }
    class RottingVignetteQueue : VignetteQueue
    {
        public RottingVignetteQueue(Rotting debuff) : base(4)
        {
            debuff.onDebuffEnd += RemoveFromQueue;
        }
        public override Color VignetteColor()
        {
            return Color.green;
        }
        public override float VignetteIntensity()
        {
            return 0.25f;
        }
    }
}
