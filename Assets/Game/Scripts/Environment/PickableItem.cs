using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PickableItem : MonoBehaviour
{
    [field: SerializeField] public Item Item { get; private set; }

    private void OnValidate()
    {
        gameObject.layer = LayerMask.NameToLayer("PickableItem");
    }
}