using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Activated_ZombieAwakener : Activated
{
    [SerializeField] Zombie[] awakeningZombie;
    [SerializeField] Animator awakenAnim;
    [SerializeField] string awakenStateName = "Awaken";
    protected override void OnActivate()
    {
        base.OnActivate();
        foreach (var i in awakeningZombie) i.Enable();
        if (awakenAnim != null)
        {
            awakenAnim.Play(awakenStateName);
        }
    }
}
