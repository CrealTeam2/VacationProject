using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Activated_ZombieActivator : Activated
{
    [SerializeField] Zombie[] activatingZombie;
    [SerializeField] float activateAmount = 100.0f;
    protected override void OnActivate()
    {
        base.OnActivate();
        foreach (var i in activatingZombie) i.AddActivation(activateAmount);
    }
}
