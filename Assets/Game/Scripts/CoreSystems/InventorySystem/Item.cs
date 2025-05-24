using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory System/Item")]
public class Item : ScriptableObject
{
    [field: SerializeField] public Sprite Sprite { get; private set; }
    
    [Tooltip("Если true, позволяет добавлять дубликаты предметов в инвентарь.")]
    [field: SerializeField] public bool AllowDuplicates { get; private set; }
}
