using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour
{
    
    [SerializeField] private ItemList itemHud;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip inventoryFullErrorSound;
    [SerializeField] private AudioClip dropItemSound;
    
    private void PlayInventoryFullSound()
    {
        if (audioSource != null && inventoryFullErrorSound != null)
        {
            audioSource.PlayOneShot(inventoryFullErrorSound);
        }
    }
    
    // INVENTROY 
    
    private Inventory inventory = new Inventory(4);
    
    public bool HasItem(ItemData itemToFind)
    {
        return inventory.Contains(itemToFind);
    }
    
    public bool HasOneOf(List<ItemData> items)
    {
        return items.Any(item => inventory.Contains(item));
    }
    
    public bool HasAll(List<ItemData> items)
    {
        return items.All(item => inventory.Contains(item));
    }
    
    public bool AddItem(ItemData item)
    {
        try
        {
            inventory.Add(item);
            itemHud.RefreshIcons();
            return true;
        }
        catch (InventoryFullException)
        {
            PlayInventoryFullSound();
            return false;
        }
    }
    
    public bool AddItem(List<ItemData> items)
    {
        try
        {
            inventory.AddRange(items);
            itemHud.RefreshIcons();
            return true;
        }
        catch (InventoryFullException)
        {
            PlayInventoryFullSound();
            return false;
        }
    }

    
    public bool RemoveItem(ItemData item)
    {
        if (!inventory.Remove(item))
            return false;

        itemHud.RefreshIcons();
        return true;
    }
    
    public bool RemoveAll(List<ItemData> items)
    {
        foreach (var item in items)
        {
            inventory.Remove(item);
        }

        itemHud.RefreshIcons();
        return !HasOneOf(items);
    }
    
    public List<ItemData> GetItems()
    {
        return inventory.Items;
    }

    public void OnCaught()
    {
        Debug.LogWarning("Player caught!");
    }
    
    public void DropItem(int itemKey)
    {
        try
        {
            var byPressedKey = inventory.FindByKeyPressed(itemKey);
            Instantiate(byPressedKey.prefab, transform.position, byPressedKey.prefab.transform.rotation);
            RemoveItem(byPressedKey);
            audioSource.PlayOneShot(dropItemSound);
        }
        catch (KeyNotMappedToItemException)
        {
            Debug.LogError("Key not mapped to item!");
            audioSource.PlayOneShot(inventoryFullErrorSound);
        }
    }
}
