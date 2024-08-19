using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabbed : Debuff
{
    public readonly EnemyTest grabber;
    const float slowScale = 0.1f;
    public Grabbed(float duration, EnemyTest grabber) : base(duration)
    {
        this.grabber = grabber;
    }
    public override void OnDebuffAdd(List<Debuff> debuffList, Player debuffed)
    {
        foreach(var i in debuffList)
        {
            if(i is Grabbed)
            {
                if((i as Grabbed).grabber == grabber)
                {
                    i.ResetDuration();
                    return;
                }
            }
        }
        base.OnDebuffAdd(debuffList, debuffed);
        debuffed.speedMultiplier *= slowScale;
        debuffed.canFocus = false;
        debuffed.canSprint = false;
        debuffed.onFistHit += Check;
    }
    void Check(EnemyTest enemy)
    {
        if(enemy == grabber)
        {
            EndDebuff();
        }
    }
    public override void OnDebuffEnd()
    {
        base.OnDebuffEnd();
        debuffed.speedMultiplier /= slowScale;
        debuffed.onFistHit -= Check;
        debuffed.canFocus = true;
        debuffed.canSprint = true;
    }
}
