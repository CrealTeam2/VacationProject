using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEngine;

public class Activated_RotStarter : Activated
{
    protected override void OnActivate()
    {
        base.OnActivate();
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().Rot();
    }
}
