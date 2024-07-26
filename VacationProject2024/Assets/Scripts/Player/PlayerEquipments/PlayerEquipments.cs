using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipments : MonoBehaviour
{
    [SerializeField] string FSMPath;
    [SerializeField] Animator m_anim;
    public Animator anim { get { return m_anim; } }
    public bool disabled = true, canSwap = true, pistolDisabled = false, acting = false;
    TopLayer<PlayerEquipments> topLayer;
    private void Awake()
    {
        topLayer = new PlayerEquipments_TopLayer(this);
        topLayer.onFSMChange += () => { FSMPath = topLayer.GetCurrentFSM(); };
        topLayer.OnStateEnter();
        FSMPath = topLayer.GetCurrentFSM();
    }
    private void Update()
    {
        topLayer.OnStateUpdate();
    }
    public void StopAction() => acting = false;
}
