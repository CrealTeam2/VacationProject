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
    public float pistolCounter = 0.0f;
    [SerializeField] int m_pistolMagSize;
    public float pistolFireRate { get { return m_pistolFireRate; } }
    public int pistolMagSize { get { return m_pistolMagSize; } }
    public Action onBulletInfoChange;
    [SerializeField] int m_pistolMag, m_bullets;
    public int pistolMag { get { return m_pistolMag; } set { m_pistolMag = value; onBulletInfoChange?.Invoke(); } }
    public int bullets { get { return m_bullets; } set { m_bullets = value; onBulletInfoChange?.Invoke(); } }
    [SerializeField] GameObject m_crosshair;
    public GameObject crosshair { get { return m_crosshair; } }

    [Header("Knife")]
    public bool hasKnife = false;

    [Header("Items")]
    public int flashGrenades = 0;
    public int bandages = 0;
    public int medicines = 0;
    public int usingItemNum = 0;

    [Header("FSMVals")]
    public int switchingTo = 0;
    public bool useItem = false;
    public int itemNum = 0;
    public bool reloadQueued = false;

    TopLayer<PlayerEquipments> topLayer;
    private void Awake()
    {
        topLayer = new PlayerEquipments_TopLayer(this);
        topLayer.onFSMChange += () => { FSMPath = topLayer.GetCurrentFSM(); };
        topLayer.OnStateEnter();
        FSMPath = topLayer.GetCurrentFSM();
        UnlockPistol();
        UnlockKnife();
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
