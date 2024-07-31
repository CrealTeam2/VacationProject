using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipments : MonoBehaviour
{
    [SerializeField] string FSMPath;
    [SerializeField] Animator m_anim;
    public Animator anim { get { return m_anim; } }
    public bool disabled = true, acting = false;
    public bool switching = false;

    [Header("Pistol")]
    public bool hasPistol = false;
    [SerializeField] float m_pistolFireRate;
    [SerializeField] int m_pistolMagSize;
    public float pistolFireRate { get { return m_pistolFireRate; } }
    public int pistolMagSize { get { return m_pistolMagSize; } }
    public int pistolMag = 0;
    public int bullets = 0;

    [Header("Knife")]
    public bool hasKnife = false;

    [Header("Items")]
    public int flashGrenades = 0;
    public int bandages = 0;
    public int medicines = 0;
    public int usingItemNum = 0;

    [Header("FSMVals")]
    public int switchingTo = 0;

    TopLayer<PlayerEquipments> topLayer;
    private void Awake()
    {
        topLayer = new PlayerEquipments_TopLayer(this);
        topLayer.onFSMChange += () => { FSMPath = topLayer.GetCurrentFSM(); };
        topLayer.OnStateEnter();
        FSMPath = topLayer.GetCurrentFSM();
        UnlockPistol();
    }
    private void Update()
    {
        topLayer.OnStateUpdate();
    }
    public void UnlockKnife()
    {
        hasKnife = true;
    }
    public void UnlockPistol()
    {
        hasPistol = true;
        pistolMag = pistolMagSize;
        bullets = pistolMagSize * 5;
    }
    public Action onClipFinish;
    public void ClipFinish() => onClipFinish.Invoke();
    public Action onFlashGrenadeUse, onBandageUse, onMedicineUse;
    public void UseFlashGrenade() => onFlashGrenadeUse.Invoke();
    public void UseBandages() => onBandageUse.Invoke();
    public void UseMedicine() => onMedicineUse.Invoke();
}
