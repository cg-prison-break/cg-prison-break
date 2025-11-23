using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    
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
            return true;
        }
        return false;
    }
    
    public List<ItemData> GetItems()
    {
        return inventory;
    }
}
