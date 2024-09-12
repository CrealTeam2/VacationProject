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
        debuffed.AddVignetteQueue(new ExhaustedVignetteQueue(this));
    }
    public override void OnDebuffEnd()
    {
        base.OnDebuffEnd();
        debuffed.speedMultiplier /= speedMultiplier;
        debuffed.breatheSFX.Play();
        debuffed.heavyBreatheSFX.Stop();
        debuffed.canSprint = true;
    }
    class ExhaustedVignetteQueue : VignetteQueue
    {
        readonly Exhausted debuff;
        public ExhaustedVignetteQueue(Exhausted debuff) : base(3)
        {
            this.debuff = debuff;
            debuff.onDebuffEnd += RemoveFromQueue;
        }
        public override Color VignetteColor()
        {
            float tmp = 1.0f - debuff.counter / debuff.duration;
            return new Color(tmp, tmp, tmp);
        }
        public override float VignetteIntensity()
        {
            return 0.25f;
        }
    }
}
