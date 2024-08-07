using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    [Header("References")]
    private PlayerController _playerController;
    private CinemachineVirtualCamera _vCamera;

    [Header("Values")]
    public float normalSensitivity = 50;
    public float aimSensitivity = 25;
    private float baseFOV;
    private float currentVerticalAngle = 0f;
    private float currentHorizontalAngle = 0f;

    [Header("FOV")]
    public float runFOV = 10f;
    public float walkFOV = 5f;
    public float crouchFOV = -5f;
    public float aimFOV = -5;

    private void Awake()
    {
        _playerController = GetComponentInParent<PlayerController>();
        _vCamera = GetComponent<CinemachineVirtualCamera>();
    }

    public void OnMoveCamera(Vector2 mouseInput)
    {
        currentHorizontalAngle += mouseInput.x * Time.deltaTime * (_playerController.isAiming ? aimSensitivity : normalSensitivity);
        currentVerticalAngle -= mouseInput.y * Time.deltaTime * (_playerController.isAiming ? aimSensitivity : normalSensitivity);
        currentVerticalAngle = Mathf.Clamp(currentVerticalAngle, -90, 90);

        transform.localRotation = Quaternion.Euler(currentVerticalAngle, currentHorizontalAngle, 0f);
    }

    void Start() {
        baseFOV = _vCamera.m_Lens.FieldOfView;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }

    void Update()
    {
        if (_playerController.isRunning)
        {
            _vCamera.m_Lens.FieldOfView = Mathf.Lerp(_vCamera.m_Lens.FieldOfView, baseFOV + runFOV, Time.deltaTime * 5f);
        }
        else if (_playerController.isWalking)
        {
            _vCamera.m_Lens.FieldOfView = Mathf.Lerp(_vCamera.m_Lens.FieldOfView, baseFOV + walkFOV, Time.deltaTime * 5f);
        }
        else if (_playerController.isCrouching)
        {
            _vCamera.m_Lens.FieldOfView = Mathf.Lerp(_vCamera.m_Lens.FieldOfView, baseFOV + crouchFOV, Time.deltaTime * 5f);
        }
        else if (_playerController.isAiming)
        {
            _vCamera.m_Lens.FieldOfView = Mathf.Lerp(_vCamera.m_Lens.FieldOfView, baseFOV + aimFOV, Time.deltaTime * 5f);
        }
        else
        {
            _vCamera.m_Lens.FieldOfView = Mathf.Lerp(_vCamera.m_Lens.FieldOfView, baseFOV, Time.deltaTime * 5f);
        }
    }
}
