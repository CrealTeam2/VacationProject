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
    void Start()
    {
        Camera = FindObjectOfType<Camera>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
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

        // 이동 입력이 있는지 확인
        canMove = _moveDirX != 0 || _moveDirZ != 0;

        if (canMove)
        {
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

            rb.MovePosition(transform.position + _velocity * Time.deltaTime);

            rb.useGravity = true;
        }
        else
        {
            if (onStair)
            {
                rb.useGravity = false;
                rb.velocity = Vector3.zero;
            }
            else
            {
                rb.useGravity = true;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Stair"))
        {
            onStair = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Stair"))
        {
            onStair = false;
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
        rb.MoveRotation(rb.rotation * Quaternion.Euler(_characterRotationY ));
    }
}
