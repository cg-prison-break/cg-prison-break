using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    public string id; // Currently unused
    public string itemName; 
    public Sprite icon; // Sprite to display in UI, May be changed dpending on the inventory UI implementation
    public GameObject prefab; // The corresponding prefab NOT scene object
}
