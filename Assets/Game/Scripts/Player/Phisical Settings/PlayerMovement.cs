using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _walkSpeed = 2f;
    [SerializeField] private PlayerCamera _playerCamera;
    
    private Rigidbody _rigidBody;
    private GameInput _gameInput;
    private Vector2 _moveInput;

    private bool _isFrozen = false;
    
    [Inject]
    public void Construct(GameInput gameInput)
    {
        _gameInput = gameInput;
    }

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        _moveInput = ReadMoveDirection();
    }
    
    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        if (_isFrozen) return;
        
        Vector3 input = new Vector3(_moveInput.x, 0f, _moveInput.y);
        Vector3 direction = transform.TransformDirection(input) * _walkSpeed;
        _rigidBody.linearVelocity = new Vector3(direction.x, _rigidBody.linearVelocity.y, direction.z);
    }

    public void FreezePlayer()
    {
        _isFrozen = true;
        _rigidBody.linearVelocity = Vector3.zero;
        _playerCamera.FreezeCamera();
    }    
    
    public void UnfreezePlayer()
    {
        _isFrozen = false;
        _playerCamera.UnfreezeCamera();
    }
    
    private Vector2 ReadMoveDirection() => _gameInput.Player.Move.ReadValue<Vector2>();
}