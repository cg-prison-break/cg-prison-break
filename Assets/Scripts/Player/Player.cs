using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    
    [SerializeField] private ItemList itemHud;
    // INVENTROY 
    
    private List<ItemData> inventory = new List<ItemData>();
    
    public bool HasItem(ItemData itemToFind)
    {
        if (inventory.Contains(itemToFind))
        {
            return true;    
        }
        return false;
    }
    
    public bool AddItem(ItemData item)
    {
        inventory.Add(item);
        itemHud.RefreshIcons();
        if (inventory.Contains(item))
        {
            return true;
        }
        return false;
    }
    
    public bool RemoveItem(ItemData item)
    {
        if (inventory.Contains(item))
        {
            inventory.Remove(item);
            itemHud.RefreshIcons();
            return true;
        }
        return false;
    }
    
    public List<ItemData> GetItems()
    {
        return inventory;
    }

    public void OnCaught()
    {
        Debug.LogWarning("Player caught!");
    }
}
