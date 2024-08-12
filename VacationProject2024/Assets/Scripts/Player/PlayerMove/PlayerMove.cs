using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour
{
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

    #region equipments
    [Header("Equipments")]
    [SerializeField] string FSMPath;
    [SerializeField] Animator m_anim;
    public Animator anim { get { return m_anim; } }
    public bool disabled = true, acting = false;
    public bool switching = false;

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
    public bool hasKnife = false;

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
    #endregion

    void Awake()
    {
        topLayer = new PlayerEquipments_TopLayer(this);
        topLayer.onFSMChange += () => { FSMPath = topLayer.GetCurrentFSM(); };
        topLayer.OnStateEnter();
        FSMPath = topLayer.GetCurrentFSM();
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
    }

    private void Move()
    {
        float _moveDirX = Input.GetAxisRaw("Horizontal");
        float _moveDirZ = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(KeyCode.LeftShift) && Stamina > 0)
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
            rb.MovePosition(transform.position + _velocity * Time.deltaTime);

            // 플레이어가 이동 중일 때 효과음 재생
            // Player 스크립트 내에서 걷기 사운드 재생
            //SoundManager.Instance.PlayWalkEffect(walkSoundClip); // walkSoundClip은 AudioClip이어야 합니다.

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

        Camera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
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
}
