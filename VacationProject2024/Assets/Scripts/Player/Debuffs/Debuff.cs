using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Debuff
{
    protected Player debuffed { get; private set; }
    public readonly float duration;
    public Debuff(float duration)
    {
        this.duration = duration;
        counter = duration;
    }
    public float counter { get; private set; }
    public void ResetDuration()
    {
        counter = duration;
    }
    public virtual void OnDebuffAdd(List<Debuff> debuffList, Player debuffed)
    {
        this.debuffed = debuffed;
        debuffList.Add(this);
        GameManager.Instance.onGameOver += EndDebuff;
    }
    public virtual void OnUpdate()
    {
        if (counter > 0) counter -= Time.deltaTime;
        else
        {
            OnDebuffTimerEnd();
        }
    }
    public bool ended { get; private set; } = false;
    protected virtual void OnDebuffTimerEnd()
    {
        EndDebuff();
    }
    public Action onDebuffEnd;
    public void EndDebuff()
    {
        if (ended) return;
        ended = true;
        OnDebuffEnd();
        debuffed.RemoveDebuff(this);
    }
    public virtual void OnDebuffEnd()
    {
        onDebuffEnd?.Invoke();
        onDebuffEnd = null;
        GameManager.Instance.onGameOver -= EndDebuff;
    }
}
