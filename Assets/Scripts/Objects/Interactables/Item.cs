using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    public string id; // don't know if necessary
    public string itemName;
    public Sprite icon;
    public GameObject prefab;
}
