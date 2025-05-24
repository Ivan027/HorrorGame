using System;
using UnityEngine;
using Zenject;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private float _mouseSensitivity = 0.5f;
    [SerializeField] private float _lookSmoothSpeed = 5f;
    [SerializeField] private Transform _cameraRoot;

    private GameInput _gameInput;
    private Vector2 _lookInput;
    private float _xRotation;

    private bool _isFrozen = false;
    private Transform _lookTarget;

    [Inject]
    public void Construct(GameInput gameInput)
    {
        _gameInput = gameInput;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (_isFrozen) return;

        if (_lookTarget != null)
        {
            SmoothLookAtTarget();
        }
        else
        {
            ManualLook();
        }
    }

    private void ManualLook()
    {
        _lookInput = ReadLookDirection() * _mouseSensitivity;

        _xRotation -= _lookInput.y;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

        _cameraRoot.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * _lookInput.x);
    }

    private void SmoothLookAtTarget()
    {
        Vector3 directionToTarget = _lookTarget.position - _cameraRoot.position;
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget.normalized);

        Vector3 euler = targetRotation.eulerAngles;
        float targetX = euler.x;
        float targetY = euler.y;

        _xRotation = Mathf.LerpAngle(_xRotation, targetX, Time.deltaTime * _lookSmoothSpeed);
        float yRotation = Mathf.LerpAngle(transform.eulerAngles.y, targetY, Time.deltaTime * _lookSmoothSpeed);

        _cameraRoot.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
        transform.rotation = Quaternion.Euler(0f, yRotation, 0f);
    }

    public void LookAt(Transform target)
    {
        _lookTarget = target;
    }

    public void SetCameraFree()
    {
        _lookTarget = null;
    }

    public void FreezeCamera()
    {
        _isFrozen = true;
    }

    public void UnfreezeCamera()
    {
        _isFrozen = false;
    }

    private Vector2 ReadLookDirection() => _gameInput.Player.Look.ReadValue<Vector2>();
}