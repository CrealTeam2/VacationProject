using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Activator : MonoBehaviour
{
    [SerializeField] Activated[] targets;
    protected void Activate()
    {
        foreach (var i in targets) i.Activate();
    }
}
