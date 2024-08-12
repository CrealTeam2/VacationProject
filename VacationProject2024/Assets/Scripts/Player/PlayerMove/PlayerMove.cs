using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
