using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class NavmeshBaker : Singleton<NavmeshBaker>
{
    NavMeshSurface surface;
    private float curTime = 0;
    [SerializeField] float coolTime;

    private void Awake()
    {
        surface = GetComponent<NavMeshSurface>();
        surface.BuildNavMesh();
    }

    private void Update()
    {
        curTime += Time.deltaTime;
        if(curTime > coolTime)
        {
            surface.BuildNavMesh();
            curTime = 0;
        }
    }
}
