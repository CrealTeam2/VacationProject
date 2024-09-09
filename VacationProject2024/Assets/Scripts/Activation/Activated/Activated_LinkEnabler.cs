using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(NavMeshLink))]
public class Activated_LinkEnabler : Activated
{
    NavMeshLink link;
    protected override bool activateOnLoad => true;
    private void Awake()
    {
        link = GetComponent<NavMeshLink>();
        link.enabled = false;
    }
    protected override void OnActivate()
    {
        base.OnActivate();
        link.enabled = true;
    }
}
