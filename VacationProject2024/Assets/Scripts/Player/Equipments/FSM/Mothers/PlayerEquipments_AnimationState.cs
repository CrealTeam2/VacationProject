using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerEquipments_AnimationState : State<Player>
{
    string clipName;
    public PlayerEquipments_AnimationState(Player origin, Layer<Player> parent, string clipName) : base(origin, parent)
    {
        this.clipName = clipName;
    }
    public override void OnStateEnter()
    {
        base.OnStateEnter();
        origin.anim.Play(clipName);
        origin.StartCoroutine(WaitClip());
    }
    IEnumerator WaitClip()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        if (origin.anim.GetCurrentAnimatorClipInfo(0)[0].clip != null)
        {
            yield return new WaitForSeconds(origin.anim.GetCurrentAnimatorClipInfo(0)[0].clip.length);
        }
        ClipFinish();
    }
    public override void OnStateUpdate()
    {
        base.OnStateUpdate();
        /*if (!set)
        {
            set = true;
            clipLength = origin.anim.GetCurrentAnimatorClipInfo(0)[0].clip.length;
            Debug.Log(clipLength);
        }
        if (counter < clipLength) counter += Time.deltaTime;
        else
        {
            ClipFinish();
        }*/
    }
    protected virtual void ClipFinish()
    {
        
    }
    public override void OnStateExit()
    {
        base.OnStateExit();
    }
}
