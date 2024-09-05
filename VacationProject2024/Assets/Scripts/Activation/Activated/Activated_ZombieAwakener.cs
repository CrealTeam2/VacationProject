using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Activated_ZombieAwakener : Activated
{
    [SerializeField] Zombie awakeningZombie;
    [SerializeField] Animator awakenAnim;
    [SerializeField] string awakenStateName = "Awaken";
    protected override void OnActivate()
    {
        base.OnActivate();
        Debug.Log(awakenAnim.gameObject.name);
        awakeningZombie.Enable();
        awakenAnim.Play("Awaken");
        if (awakenAnim != null)
        {
            Debug.Log(awakenStateName);
            awakenAnim.Play(awakenStateName);
        }
    }
}
