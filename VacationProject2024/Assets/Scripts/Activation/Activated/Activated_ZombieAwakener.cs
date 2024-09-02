using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Activated_ZombieAwakener : Activated
{
    [SerializeField] Zombie awakeningZombie;
    [SerializeField] Animator awakenAnimation;
    [SerializeField] string awakenStateName = "Awaken";
    protected override void OnActivate()
    {
        awakeningZombie.onEnable += base.OnActivate;
        awakenAnimation.Play(awakenStateName);
    }
}
