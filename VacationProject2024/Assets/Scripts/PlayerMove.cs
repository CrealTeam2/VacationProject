using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float walkSpeed;
    [SerializeField] public float Stamina = 100;
    [SerializeField] private float lookSensitivity;

    [SerializeField] private float cameraRotationLimit;
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
        if (Input.GetKey(KeyCode.LeftShift) && Stamina>0)
        {
            walkSpeed = 120;
            Stamina -= Time.deltaTime * 20;
        }
        else
        {
            walkSpeed = 50;
            if (Stamina <= 100) 
            {
                Stamina += Time.deltaTime *10 ;
            }
        }
        float _moveDirX = Input.GetAxisRaw("Horizontal");
        float _moveDirZ = Input.GetAxisRaw("Vertical");

        Vector3 _moveHorizontal = transform.right * _moveDirX;
        Vector3 _moveVertical = transform.forward * _moveDirZ;

        Vector3 _velocity = ( _moveHorizontal + _moveVertical ).normalized * walkSpeed;

        rb.MovePosition(transform.position + _velocity * Time.deltaTime);
    }

    private void CameraRotation()
    
    {
        float _xRotation = Input.GetAxisRaw("Mouse Y");
        float _cameraRotationX = _xRotation * lookSensitivity;
        currentCameraRotationX -= _cameraRotationX;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);

        Camera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
    }
    private void CharacterRotation()
    {
        float _yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 _characrerRotationY = new Vector3(0f, _yRotation, 0f) * lookSensitivity;
        rb.MoveRotation(rb.rotation * Quaternion.Euler(_characrerRotationY));

    }
}
