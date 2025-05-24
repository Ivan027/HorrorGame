using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class InventoryDisplay : MonoBehaviour
{
    [SerializeField] private Image[] _images = new Image[5];

    private InventoryController _inventoryController;
    
    [Inject]
    public void Construct(InventoryController inventoryController)
    {
        _inventoryController = inventoryController;
    }

    private void Awake()
    {
        DisplayInventoryController(null);
    }

    private void OnEnable()
    {
        _inventoryController.OnItemAdded += DisplayInventoryController;
    }

    private void OnDisable()
    {
        _inventoryController.OnItemAdded -= DisplayInventoryController;
    }

    private void DisplayInventoryController(Item item)
    {
        for (int i = 0; i < _images.Length; i++)
        {
            if (_inventoryController.GetItemsCount() > i)
            {
                _images[i].gameObject.SetActive(true);
                _images[i].sprite = _inventoryController.GetItem(i).Sprite;
                continue;
            }
            
            _images[i].gameObject.SetActive(false);
            _images[i].sprite = null;
        }
    }
}