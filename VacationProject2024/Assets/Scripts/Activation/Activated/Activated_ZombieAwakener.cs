using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Activated_ZombieAwakener : Activated
{
    [SerializeField] Zombie awakeningZombie;
    [SerializeField] Animator awakenAnimation;
    protected override void OnActivate()
    {
        awakeningZombie.onEnable += base.OnActivate;
        awakenAnimation.SetTrigger("Awaken");
    }
}
