using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class NavmeshBaker : Singleton<NavmeshBaker>
{
    Transform player; 
    NavMeshSurface surface;
    private float curTime = 0;
    [SerializeField] float coolTime;

    private void Awake()
    {

        surface = GetComponent<NavMeshSurface>();
        surface.BuildNavMesh();
        player = GameObject.FindWithTag("Player").transform;
        BakeNavmesh();
    }

    private void Update()
    {
        curTime += Time.deltaTime;
        if(curTime > coolTime)
        {
            BakeNavmesh();
            curTime = 0;
        }
    }

    void BakeNavmesh()
    {
        surface.size = new Vector3(20, 20, 20);
        surface.center = player.position;
        surface.BuildNavMesh();
    }
}
