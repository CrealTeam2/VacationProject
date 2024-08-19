using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class NavmeshBaker : Singleton<NavmeshBaker>
{
    NavMeshSurface surface;

    private void Awake()
    {
        surface = GetComponent<NavMeshSurface>();
        surface.BuildNavMesh();
    }

    private void FixedUpdate()
    {
/*        surface.BuildNavMesh();*/
    }
}
