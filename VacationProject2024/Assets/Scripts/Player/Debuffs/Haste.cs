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
        foreach(var i in debuffList)
        {
            if (i is Exhausted) i.EndDebuff();
        }
        debuffed.speedMultiplier *= speedMultiplier;
        debuffed.hastened = true;
    }
    public override void OnDebuffEnd()
    {
        base.OnDebuffEnd();
        debuffed.speedMultiplier /= speedMultiplier;
        debuffed.hastened = false;
    }
}
