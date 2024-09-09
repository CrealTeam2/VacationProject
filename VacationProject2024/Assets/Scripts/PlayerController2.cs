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
    private int currentPaperNumber = -1; // 현재 상호작용 중인 Paper 번호

    private bool canMove = true;
    private bool nearPaper = false;
    private UIController uiController;

    void Start()
    {
        myRigid = GetComponent<Rigidbody>();
        lookSensitivity = PlayerPrefs.GetFloat("Sensitivity", lookSensitivity);
        canMove = false;
        uiController = FindObjectOfType<UIController>();
    }

    void Update()
    {
        if (canMove)
        {
            Move();
            CameraRotation();
            CharacterRotation();
            if (nearPaper && Input.GetKeyDown(KeyCode.F))
            {
                if (uiController != null && currentPaperNumber > 0)
                {
                    uiController.ShowSpecificPaperlist(currentPaperNumber);
                }
            }
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
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Paper"))
        {
            nearPaper = true;
            Paper paper = other.GetComponent<Paper>();
            if (paper != null)
            {
                currentPaperNumber = paper.paperNumber;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Paper"))
        {
            nearPaper = false;
            currentPaperNumber = -1;
        }
    }


}