using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class PlayerItemPicker : MonoBehaviour
{
    [Header("Pick Up Settings")]
    [SerializeField] private float _pickUpDistance;
    [SerializeField] private LayerMask _pickableLayer;
    
    [Header("Crosshair Settings")]
    [SerializeField] private PlayerCrosshairDisplay _playerCrosshairDisplay;
    [SerializeField] private Sprite _crosshairPickUpSprite;
    
    private Camera _camera;
    private InventoryController _inventoryController;
    private GameInput _gameInput;
    
    private PickableItem _currentSelectedItem;
    
    [Inject]
    public void Construct(InventoryController inventoryController, GameInput gameInput)
    {
        _inventoryController = inventoryController;
        _gameInput = gameInput;
    }

    private void Awake()
    {
        _camera = Camera.main;
    }

    private void OnEnable()
    {
        _gameInput.Player.Interact.performed += AddSelectedItem;
    }

    private void OnDisable()
    {
        _gameInput.Player.Interact.performed -= AddSelectedItem;
    }

    private void Update()
    {
        SetSelectedItem();
    }

    private void SetSelectedItem()
    {
        Ray ray = _camera.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f));

        if (Physics.Raycast(ray, out RaycastHit hit, _pickUpDistance, _pickableLayer) &&
            hit.collider.TryGetComponent(out PickableItem item))
        {
            _currentSelectedItem = item;
            _playerCrosshairDisplay.SetCrosshair(null, Color.red);
            _playerCrosshairDisplay.ShowText("Take");
        }
        else
        {
            _currentSelectedItem = null;
            _playerCrosshairDisplay.ReturnDefaultCrosshair();
            _playerCrosshairDisplay.HideText();
        }

    }

    private void AddSelectedItem(InputAction.CallbackContext ctx)
    {
        if (_currentSelectedItem == null) return;

        _inventoryController.AddItem(_currentSelectedItem.Item);
        _currentSelectedItem.gameObject.SetActive(false);
    }
}