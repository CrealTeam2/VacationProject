using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Animation))]
public class Activated_HasteGiver : Activated
{
    [SerializeField] float duration, speedMultiplier;
    protected override void OnActivate()
    {
        base.OnActivate();
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().AddDebuff(new Haste(speedMultiplier, duration));
    }
}
