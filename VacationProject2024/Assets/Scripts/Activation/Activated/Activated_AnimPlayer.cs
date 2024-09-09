using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Animation))]
public class Activated_AnimPlayer : Activated
{
    [SerializeField] string clipName;
    Animation anim;
    protected override bool activateOnLoad => true;
    private void Awake()
    {
        anim = GetComponent<Animation>();
    }
    protected override void OnActivate()
    {
        base.OnActivate();
        anim.Play(clipName);
    }
}
