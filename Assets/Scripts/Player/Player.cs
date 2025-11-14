using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    
    
    
    // INVENTROY 
    
    private List<Item> inventory = new List<Item>();
    
    public bool HasItem(Item itemToFind)
    {
        if (inventory.Contains(itemToFind))
        {
            return true;    
        }
        return false;
    }
    
    public bool AddItem(Item item)
    {
        inventory.Add(item);
        if (inventory.Contains(item))
        {
            return true;
        }
        return false;
    }
    
    public bool RemoveItem(Item item)
    {
        inventory.Remove(item);
        if (inventory.Contains(item))
        {
            return false;
        }
        return true;
    }
    
    public List<Item> GetItems()
    {
        return inventory;
    }
}
