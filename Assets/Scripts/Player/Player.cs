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

    [Header("Inventory-Sounds")]
    [SerializeField] private AudioSource dropItemSound;
    [SerializeField] private AudioSource pickupItemSound;
    [SerializeField] private AudioSource inventoryFullItemNotDroppableSound;

    [Header("Game-Sounds")]
    [SerializeField] private AudioClip caughtAudioClip;
    [SerializeField] private AudioClip gameOverAudioClip;

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

    public bool AddItem(ItemData item, bool playSound = true)
    {
        for (var i = 0; i < _inventory.Length; i++)
        {
            if (_inventory[i] != null) continue;
            _inventory[i] = item;
            itemHud.RefreshIcons();
            if (playSound)
                pickupItemSound.Play();
            ShowNewItemHint(item);
            GameTelemetryLogger.LogTelemetryEvent(new ItemPickedUpData(item.itemName));
            return true;
        }

        // No free slot found
        inventoryFullItemNotDroppableSound.Play();
        return false;
    }

    public bool AddItem(List<ItemData> items)
    {
        return items.All(item => AddItem(item, false));
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

    private bool DropItemFromSlot(int index)
    {
        if (index < 0 || index >= _inventory.Length)
            return false;

        if (_inventory[index] == null)
        {
            inventoryFullItemNotDroppableSound.Play();
            return false;
        }

        Instantiate(_inventory[index].prefab, transform.position + transform.forward, Quaternion.identity);
        GameTelemetryLogger.LogTelemetryEvent(new ItemDroppedData(_inventory[index].name, index));
        _inventory[index] = null;
        itemHud.RefreshIcons();
        dropItemSound.Play();
        return true;
    }

    public bool AddItemToSlot(ItemData item, int index)
    {
        if (index < 0 || index >= _inventory.Length)
            return false;

        if (_inventory[index] != null)
            return false;

        _inventory[index] = item;
        GameTelemetryLogger.LogTelemetryEvent(new ItemPickedUpData(item.itemName));
        itemHud.RefreshIcons();
        return true;
    }

    public void OnCaught(AudioSource audioSource)
    {
        if (gameData.strikes >= 3)
        {
            if (audioSource != null && gameOverAudioClip != null)
            {
                audioSource.PlayOneShot(gameOverAudioClip);
            }
            EndingContext.NextEnding = EndingType.Bad;
            SceneManager.LoadScene(GameScene.Ending);
        }
        else
        {
            if (audioSource != null && caughtAudioClip != null)
            {
                audioSource.PlayOneShot(caughtAudioClip);
            }
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
            _selectedSlot = (_selectedSlot - 1 + _inventory.Length) % _inventory.Length;
        else
            _selectedSlot = (_selectedSlot + 1) % _inventory.Length;

        itemHud.UpdateSelectedSlot(_selectedSlot);
        Debug.Log($"Selected Slot: {_selectedSlot + 1}");
    }

    public void DropItem()
    {
        DropItemFromSlot(_selectedSlot);
    }

    public bool IsInventoryFull()
    {
        return _inventory.All(item => item != null);
    }

    public void ShowNewItemHint(ItemData item)
    {
        string name = item.itemName.Contains("karte") ? "Sicherheitskarte" : item.itemName;
        if (gameData.collectedItems.Contains(name)) return;
        gameData.collectedItems.Add(name);
        itemHud.ShowNewItemHint();
    }
}