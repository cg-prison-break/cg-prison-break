using System.Collections.Generic;
using System.Linq;
using ui.Hud;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    
    [SerializeField] private ItemList itemHud;
    
    [SerializeField] private GameData gameData;
    [SerializeField] private PauseMenu pauseMenuManager;
    
    // INVENTORY (8 feste Slots)
    private int _maxSlots = 8;
    private ItemData[] _inventory;

    private int _selectedSlot = 0;

    public void Awake()
    {
        _inventory = new ItemData[_maxSlots];
        PlayerRegistry.RegisterPlayer(this);
    }
    
    public void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public bool HasItem(ItemData itemToFind)
    {
        return _inventory.Any(item => item == itemToFind);
    }
    
    public bool HasOneOf(List<ItemData> items)
    {
        return items.Any(HasItem);
    }
    
    public bool HasAll(List<ItemData> items)
    {
        return items.All(HasItem);
    }
    
    public bool AddItem(ItemData item)
    {
        for (var i = 0; i < _inventory.Length; i++)
        {
            if (_inventory[i] != null) continue;
            _inventory[i] = item;
            itemHud.RefreshIcons();
            return true;
        }
        
        // No free slot found
        return false;
    }
    
    public bool AddItem(List<ItemData> items)
    {
        return items.All(AddItem);
    }
    
    public bool RemoveItem(ItemData item)
    {
        for (var i = 0; i < _inventory.Length; i++)
        {
            if (_inventory[i] != item) continue;
            _inventory[i] = null;
            itemHud.RefreshIcons();
            return true;
        }
        return false;
    }
    
    public bool RemoveAll(List<ItemData> items)
    {
        return items.Aggregate(false, (current, item) => current | RemoveItem(item));
    }
    
    public ItemData[] GetItems()
    {
        return _inventory;
    }
    
    public bool IsSlotEmpty(int index)
    {
        if (index < 0 || index >= _inventory.Length)
            return true;

        return _inventory[index] == null;
    }

    private bool RemoveItemFromSlot(int index)
    {
        if (index < 0 || index >= _inventory.Length)
            return false;

        if (_inventory[index] == null)
            return false;

        Instantiate(_inventory[index].prefab, transform.position + transform.forward, Quaternion.identity);
        _inventory[index] = null;
        itemHud.RefreshIcons();
        return true;
    }
    
    public bool AddItemToSlot(ItemData item, int index)
    {
        if (index < 0 || index >= _inventory.Length)
            return false;

        if (_inventory[index] != null)
            return false;

        _inventory[index] = item;
        itemHud.RefreshIcons();
        return true;
    }

    public void OnCaught()
    {
        if (gameData.strikes >= 3)
        {
            EndingContext.NextEnding = EndingType.Bad;
            SceneManager.LoadScene(GameScene.Ending);
        }
        else
        {
            pauseMenuManager.OpenRetryMenu();
        }
    }
    
    public void SelectSlot(float slotValue)
    {
        var slot = Mathf.RoundToInt(slotValue) - 1;

        if (slot < 0 || slot >= _inventory.Length) return;
        _selectedSlot = slot;
        
        itemHud.UpdateSelectedSlot(_selectedSlot);
        Debug.Log($"Selected Slot: {_selectedSlot + 1}");
    }

    public void ScrollSlot(float scroll)
    {
        if (Mathf.Abs(scroll) < 0.1f)
            return;

        if (scroll > 0)
            _selectedSlot = (_selectedSlot - 1) % _inventory.Length;
        else
            _selectedSlot = (_selectedSlot + 1 + _inventory.Length) % _inventory.Length;

        itemHud.UpdateSelectedSlot(_selectedSlot);
        Debug.Log($"Selected Slot: {_selectedSlot + 1}");
    }

    public void DropItem()
    {
        RemoveItemFromSlot(_selectedSlot);
    }
}
