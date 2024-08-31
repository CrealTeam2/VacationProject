using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using static UnityEngine.UI.Image;

public class Player : MonoBehaviour, ISavable
{
    [Header("Hp")]
    [SerializeField] float m_maxHp;
    [SerializeField] float m_hp;
    public float maxHp { get { return m_maxHp; } }
    public float hp { get { return m_hp; } private set { m_hp = value; } }

    [Header("Movement")]
    [SerializeField] string MovementFSMPath;
    [SerializeField] Transform rotator;
    [SerializeField] public float Stamina = 100;
    [SerializeField] private float lookSensitivity;
    //private float walkSpeed;
    public float lowerCameraRotationLimit = 60f;
    public float upperCameraRotationLimit = -60f;
    private bool canMove = true;
    private bool onStair = false;
    private float currentCameraRotationX = 0f;

    [SerializeField] float m_walkSpeed, m_runSpeed, m_maxStamina;
    public float walkSpeed { get { return m_walkSpeed; } }
    public float runSpeed { get { return m_runSpeed; } }
    public float maxStamina { get { return m_maxStamina; } }

    [SerializeField]
    private Camera Camera;
    private Rigidbody rb;
    public bool isGrounded { get; private set; }
    private float slopeLimit = 45f;

    public float speedMultiplier = 1.0f;
    TopLayer<Player> movementTopLayer;

    #region equipments
    [Header("Equipments")]
    [SerializeField] string FSMPath;
    [SerializeField] Animator m_anim;
    public Animator anim { get { return m_anim; } }
    public bool disabled = true, acting = false;
    public bool switching = false;

    [Header("Unarmed")]
    [SerializeField] float fistDamage;
    [SerializeField] EnemyDetector m_rightFistHitbox;
    [SerializeField] EnemyDetector m_leftFistHitbox;
    public EnemyDetector rightFistHitbox { get { return m_rightFistHitbox; } }
    public EnemyDetector leftFistHitbox { get { return m_leftFistHitbox; } }
    public Action<EnemyTest> onFistHit;

    [Header("Pistol")]
    [SerializeField] GameObject m_pistolModel;
    [SerializeField] float m_pistolDamage;
    [SerializeField] float m_pistolFireRate, m_pistolFocusFireRate;
    [SerializeField] int m_pistolMagSize;
    [SerializeField] int m_pistolMag, m_bullets;
    [SerializeField] Transform m_firePoint;
    [SerializeField] GameObject m_crosshair;
    public Action onBulletInfoChange;
    public float pistolCounter = 0.0f;
    public float pistolDamage { get { return m_pistolDamage; } }
    public float pistolFireRate { get { return m_pistolFireRate; } }
    public float pistolFocusFireRate { get { return m_pistolFocusFireRate; } }
    public int pistolMagSize { get { return m_pistolMagSize; } }
    public int pistolMag { get { return m_pistolMag; } set { m_pistolMag = value; onBulletInfoChange?.Invoke(); } }
    public int bullets { get { return m_bullets; } set { m_bullets = value; onBulletInfoChange?.Invoke(); } }
    public bool hasPistol { get; private set; } = false;
    public Transform firePoint { get { return m_firePoint; } }
    public GameObject crosshair { get { return m_crosshair; } }
    public GameObject pistolModel { get { return m_pistolModel; } }

    [Header("Knife")]
    [SerializeField] float knifeDamage;
    [SerializeField] EnemyDetector m_knifeHitbox;
    public EnemyDetector knifeHitbox { get { return m_knifeHitbox; } }
    public Action<EnemyTest> onKnifeHit;
    public bool hasKnife { get; private set; } = false;

    [Header("Items")]
    public int flashGrenades = 0;
    public int bandages = 0;
    public int medicines = 0;
    public int usingItemNum = 0;

    [Header("FSMVals")]
    public bool punchedRight = false;
    public int switchingTo = 0;
    public bool useItem = false;
    public int itemNum = 0;
    public bool reloadQueued = false;

    TopLayer<Player> topLayer;
    public const float focusSlowScale = 0.5f;
    #endregion

    List<Debuff> debuffs = new();
    //Restrictions
    int m_cantSprint = 0, m_cantFocus = 0;
    public bool canSprint { get { return m_cantSprint <= 0; } set { if (value == false) m_cantSprint++; else m_cantSprint--; } }
    public bool canFocus { get { return m_cantFocus <= 0; } set { if (value == false) m_cantFocus++; else m_cantFocus--; } }
    void Awake()
    {
        hp = maxHp;
        //topLayer = new PlayerEquipments_TopLayer(this);
        //topLayer.onFSMChange += () => { FSMPath = topLayer.GetCurrentFSM(); };
        //topLayer.OnStateEnter();
        movementTopLayer = new PlayerMovements_TopLayer(this);
        movementTopLayer.onFSMChange += () => { MovementFSMPath = movementTopLayer.GetCurrentFSM(); };
        movementTopLayer.OnStateEnter();
        //FSMPath = topLayer.GetCurrentFSM();
        rightFistHitbox.onHit += FistHit;
        leftFistHitbox.onHit += FistHit;
/*        UnlockPistol();
        UnlockKnife();*/
    }
    void Start()
    {
        Camera = FindObjectOfType<Camera>();
        rb = GetComponent<Rigidbody>();

        //SoundManager.Instance.PlaySound("TestBGM", SoundManager.Instance.BGMVolume, 0);

    }

    void Update()
    {
        Move();
        CameraRotation();
        CharacterRotation();
        //topLayer.OnStateUpdate();
        pistolCounter += Time.deltaTime;
        foreach (var i in debuffs) i.OnUpdate();
        if(removeQueue.Count > 0)
        {
            debuffs.RemoveAll((Debuff i) => { return removeQueue.Contains(i); });
            removeQueue.Clear();
        }
    }

    private void Move()
    {
        movementTopLayer.OnStateUpdate();
    }

    public void MovePos(Vector3 translation)
    {
        rb.MovePosition(transform.position + translation * speedMultiplier);
    }

    private void CameraRotation()
    {
        float _xRotation = Input.GetAxisRaw("Mouse Y");
        float _cameraRotationX = _xRotation * lookSensitivity;
        currentCameraRotationX -= _cameraRotationX;

        if (currentCameraRotationX > upperCameraRotationLimit)
        {
            currentCameraRotationX = upperCameraRotationLimit;
        }
        else if (currentCameraRotationX < lowerCameraRotationLimit)
        {
            currentCameraRotationX = lowerCameraRotationLimit;
        }

        rotator.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
    }

    private void CharacterRotation()
    {
        float _yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 _characterRotationY = new Vector3(0f, _yRotation, 0f) * lookSensitivity;
        rb.MoveRotation(rb.rotation * Quaternion.Euler(_characterRotationY));
    }

    private void FixedUpdate()
    {
        isGrounded = Physics.Raycast(transform.position + Vector3.up * 2.0f, Vector3.down, out RaycastHit hit, 2.3f);
        if (isGrounded)
        {
            float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);
            if (slopeAngle <= slopeLimit)
            {
                rb.useGravity = true;
            }
            else
            {
                rb.useGravity = false;
            }
        }
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
    public void ClipFinish() => onClipFinish?.Invoke();
    public Action onFlashGrenadeUse, onBandageUse, onMedicineUse;
    public void UseFlashGrenade() => onFlashGrenadeUse.Invoke();
    public void UseBandages() => onBandageUse.Invoke();
    public void UseMedicine() => onMedicineUse.Invoke();
    public void FirePistol()
    {
        RaycastHit hit;
        if (Physics.Raycast(firePoint.position, firePoint.forward, out hit, Mathf.Infinity, LayerMask.GetMask("Enemy")))
        {
            hit.transform.GetComponent<EnemyTest>()?.GetDamage(pistolDamage);
        }
    }

    public Action onDamage;
    public void GetDamage(float damage)
    {
        hp = Mathf.Max(0, hp - damage);
        onDamage?.Invoke();
        if(hp <= 0)
        {
            //gameover
        }
    }
    public void FistHit(EnemyTest enemy)
    {
        onFistHit?.Invoke(enemy);
        enemy.GetDamage(fistDamage);
    }
    public void KnifeHit(EnemyTest enemy)
    {
        onKnifeHit?.Invoke(enemy);
        enemy.GetDamage(knifeDamage);
    }

    public void AddDebuff(Debuff debuff)
    {
        debuff.OnDebuffAdd(debuffs, this);
    }
    List<Debuff> removeQueue = new();
    public void RemoveDebuff(Debuff debuff)
    {
        removeQueue.Add(debuff);
    }

    public void LoadData(Database data)
    {
        transform.position = data.savePoint;
    }

    public void SaveData(ref Database data)
    {

    }
}
[CustomEditor(typeof(Player))]
public class Player_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Give Rotting Debuff")) (target as Player).AddDebuff(new Rotting());
        if (GUILayout.Button("Give Injured Debuff")) (target as Player).AddDebuff(new Injured(10.0f));
    }
}