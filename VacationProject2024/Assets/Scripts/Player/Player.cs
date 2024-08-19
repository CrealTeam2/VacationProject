using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class Player : MonoBehaviour
{
    [Header("Hp")]
    [SerializeField] float m_maxHp;
    [SerializeField] float m_hp;
    public float maxHp { get { return m_maxHp; } }
    public float hp { get { return m_hp; } private set { m_hp = value; } }

    [Header("Movement")]
    [SerializeField] Transform rotator;
    [SerializeField] private float walkSpeed;
    [SerializeField] public float Stamina = 100;
    [SerializeField] private float lookSensitivity;
    public float lowerCameraRotationLimit = 60f;
    public float upperCameraRotationLimit = -60f;
    private bool canMove = true;
    private bool onStair = false;
    private float currentCameraRotationX = 0f;

    [SerializeField]
    private Camera Camera;
    private Rigidbody rb;
    private bool isGrounded;
    private float slopeLimit = 45f;

    public float speedMultiplier = 1.0f;

    #region equipments
    [Header("Equipments")]
    [SerializeField] string FSMPath;
    [SerializeField] Animator m_anim;
    public Animator anim { get { return m_anim; } }
    public bool disabled = true, acting = false;
    public bool switching = false;

    [Header("Unarmed")]
    [SerializeField] float fistDamage;
    [SerializeField] EnemyDetector rightFistHitbox;
    [SerializeField] EnemyDetector leftFistHitbox;
    public Action<EnemyTest> onFistHit;

    [Header("Pistol")]
    public bool hasPistol = false;
    [SerializeField] float m_pistolDamage;
    public float pistolDamage { get { return m_pistolDamage; } }
    [SerializeField] float m_pistolFireRate;
    public float pistolCounter = 0.0f;
    [SerializeField] int m_pistolMagSize;
    public float pistolFireRate { get { return m_pistolFireRate; } }
    public int pistolMagSize { get { return m_pistolMagSize; } }
    public Action onBulletInfoChange;
    [SerializeField] int m_pistolMag, m_bullets;
    public int pistolMag { get { return m_pistolMag; } set { m_pistolMag = value; onBulletInfoChange?.Invoke(); } }
    public int bullets { get { return m_bullets; } set { m_bullets = value; onBulletInfoChange?.Invoke(); } }
    [SerializeField] Transform m_firePoint;
    public Transform firePoint { get { return m_firePoint; } }
    [SerializeField] GameObject m_crosshair;
    public GameObject crosshair { get { return m_crosshair; } }

    [Header("Knife")]
    [SerializeField] float knifeDamage;
    [SerializeField] EnemyDetector knifeHitbox;
    public bool hasKnife = false;
    public Action<EnemyTest> onKnifeHit;

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
        topLayer = new PlayerEquipments_TopLayer(this);
        topLayer.onFSMChange += () => { FSMPath = topLayer.GetCurrentFSM(); };
        topLayer.OnStateEnter();
        FSMPath = topLayer.GetCurrentFSM();
        rightFistHitbox.onHit += (EnemyTest enemy) => { onFistHit?.Invoke(enemy); enemy.GetDamage(fistDamage); };
        leftFistHitbox.onHit += (EnemyTest enemy) => { onFistHit?.Invoke(enemy); enemy.GetDamage(fistDamage); };
        knifeHitbox.onHit += (EnemyTest enemy) => { onKnifeHit?.Invoke(enemy); enemy.GetDamage(knifeDamage); };
        UnlockPistol();
        UnlockKnife();
    }
    void Start()
    {
        Camera = FindObjectOfType<Camera>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Move();
        CameraRotation();
        CharacterRotation();
        Debug.Log(Stamina);
        topLayer.OnStateUpdate();
        foreach (var i in debuffs) i.OnUpdate();
        if(removeQueue.Count > 0)
        {
            debuffs.RemoveAll((Debuff i) => { return removeQueue.Contains(i); });
            removeQueue.Clear();
        }
    }

    private void Move()
    {
        float _moveDirX = Input.GetAxisRaw("Horizontal");
        float _moveDirZ = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(KeyCode.LeftShift) && Stamina > 0 && canSprint)
        {
            walkSpeed = 120;
            Stamina -= Time.deltaTime * 20;
        }
        else
        {
            walkSpeed = 50;
            if (Stamina <= 100)
            {
                Stamina += Time.deltaTime * 10;
            }
        }

        Vector3 _moveHorizontal = transform.right * _moveDirX;
        Vector3 _moveVertical = transform.forward * _moveDirZ;

        Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized * walkSpeed;

        if (_velocity.magnitude > 0 && isGrounded)
        {
            rb.MovePosition(transform.position + _velocity * Time.deltaTime * speedMultiplier);
            if (!canMove)
            {
                SoundManager.Instance.PlaySound("Walk",SoundManager.Instance.MasterVolume, 0); // 무한 반복 재생
                canMove = true;
            }
        }
        else
        {
            if (canMove)
            {
                SoundManager.Instance.StopSound("Walk");
                canMove = false;
            }
        }
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
        isGrounded = Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 1.1f);

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
    public void ClipFinish() => onClipFinish.Invoke();
    public Action onFlashGrenadeUse, onBandageUse, onMedicineUse;
    public void UseFlashGrenade() => onFlashGrenadeUse.Invoke();
    public void UseBandages() => onBandageUse.Invoke();
    public void UseMedicine() => onMedicineUse.Invoke();

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

    public void AddDebuff(Debuff debuff)
    {
        debuff.OnDebuffAdd(debuffs, this);
    }
    List<Debuff> removeQueue = new();
    public void RemoveDebuff(Debuff debuff)
    {
        removeQueue.Add(debuff);
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