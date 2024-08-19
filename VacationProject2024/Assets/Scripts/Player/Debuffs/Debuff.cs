using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEditor.Build.Content;
using UnityEngine;

public abstract class Debuff
{
    protected Player debuffed { get; private set; }
    readonly float duration;
    public Debuff(float duration)
    {
        this.duration = duration;
        counter = duration;
    }
    float counter;
    public void ResetDuration()
    {
        counter = duration;
    }
    public virtual void OnDebuffAdd(List<Debuff> debuffList, Player debuffed)
    {
        this.debuffed = debuffed;
        debuffList.Add(this);
        //GameManager.Instance.onGameOver += EndDebuff;
    }
    public virtual void OnUpdate()
    {
        if (counter > 0) counter -= Time.deltaTime;
        else
        {
            EndDebuff();
        }
    }
    bool ended = false;
    public void EndDebuff()
    {
        if (ended) return;
        ended = true;
        OnDebuffEnd();
        debuffed.RemoveDebuff(this);
    }
    public virtual void OnDebuffEnd()
    {
        //GameManager.Instance.onGameOver -= EndDebuff;
    }
}
