using System;
using System.Collections.Generic;

public class InventoryController
{
    private List<Item> _items = new List<Item>();
    public int InventorySize { get; private set; } = 0;

    public event Action<Item> OnItemAdded; 
    public event Action<Item> OnItemRemoved; 
    
    public InventoryController(int inventorySize, List<Item> startItems)
    {
        InventorySize = inventorySize;

        for (int i = 0; i < startItems.Count; i++)
        {
            AddItem(startItems[i]);
        }
    }

    public void AddItem(Item item)
    {
        if (item == null || item.AllowDuplicates == false && _items.Contains(item) || _items.Count >= InventorySize) return;

        _items.Add(item);
        OnItemAdded?.Invoke(item);
        
    }

    public void RemoveItem(Item item)
    {
        _items.Remove(item);
        OnItemRemoved?.Invoke(item);
    }

    public Item GetItem(int slotIndex)
    {
        return _items[slotIndex];
    }

    public int GetItemsCount()
    {
        return _items.Count;
    }
}