using System;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    
    [SerializeField] private ItemList itemHud;
    
    [SerializeField] private GameData gameData;
    // INVENTROY 
    
    private List<ItemData> inventory = new List<ItemData>(5);

    public void Awake()
    {
        PlayerRegistry.RegisterPlayer(this);
    }

    public bool HasItem(ItemData itemToFind)
    {
        if (inventory.Contains(itemToFind))
        {
            return true;    
        }
        return false;
    }
    
    public bool HasOneOf(List<ItemData> items)
    {
        foreach (ItemData item in items)
        {
            if (inventory.Contains(item))
            {
                return true;
            }
        }
        return false;
    }
    
    public bool HasAll(List<ItemData> items)
    {
        foreach (ItemData item in items)
        {
            if (!inventory.Contains(item))
            {
                return false;
            }
        }
        return true;
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
    
    public bool AddItem(List<ItemData> items)
    {
        inventory.AddRange(items);
        itemHud.RefreshIcons();
        return HasAll(items);
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
    
    public bool RemoveAll(List<ItemData> items)
    {
        foreach (ItemData item in items)
        {
            inventory.Remove(item);
        }
        itemHud.RefreshIcons();
        return !HasOneOf(items);
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
