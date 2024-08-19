using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float walkSpeed;
    [SerializeField] private Camera theCamera;
    private Rigidbody myRigid;
    [SerializeField] private float lookSensitivity;
    [SerializeField] private float cameraRotationLimit;
    private float currentCameraRotationX = 0;

    private bool canMove = true; // 플레이어의 움직임을 제어하는 변수

    void Start()
    {
        myRigid = GetComponent<Rigidbody>();
        lookSensitivity = PlayerPrefs.GetFloat("Sensitivity", lookSensitivity);
    }

    void Update()
    {
        if (canMove) // 움직일 수 있는 경우에만 Move, CameraRotation, CharacterRotation 호출
        {
            Move();
            CameraRotation();
            CharacterRotation();
        }
    }

    private void Move()
    {
        float _moveDirX = Input.GetAxisRaw("Horizontal");
        float _moveDirZ = Input.GetAxisRaw("Vertical");

        Vector3 _moveHorizontal = transform.right * _moveDirX;
        Vector3 _moveVertical = transform.forward * _moveDirZ;
        Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized * walkSpeed;
        myRigid.MovePosition(transform.position + _velocity * Time.deltaTime);
    }

    private void CameraRotation()
    {
        float _xRotation = Input.GetAxisRaw("Mouse Y");
        float _cameraRotationX = _xRotation * lookSensitivity;
        currentCameraRotationX -= _cameraRotationX;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);

        theCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
    }

    private void CharacterRotation()
    {
        float _yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 _characterRotationY = new Vector3(0f, _yRotation, 0f) * lookSensitivity;
        myRigid.MoveRotation(myRigid.rotation * Quaternion.Euler(_characterRotationY));
    }

    public void SetMovementEnabled(bool isEnabled)
    {
        canMove = isEnabled;
    }


    public void UpdateSensitivity(float newSensitivity)
    {
        lookSensitivity = newSensitivity;
    }
}
