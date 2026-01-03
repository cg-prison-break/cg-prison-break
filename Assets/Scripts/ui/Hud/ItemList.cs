using UnityEngine;
using UnityEngine.UI;

namespace ui.Hud
{
    public class ItemList : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private GameObject itemPanel;
    [SerializeField] private Image itemIconPrefab;
    [SerializeField] private Vector2 startOffset = new Vector2(30f, -30f);
    [SerializeField] private Vector2 spacing = new Vector2(55f, -55f);
    [SerializeField] private int itemsPerRow = 4;
    
    private int _fixedInventorySize = 8;

    private void OnEnable()
    {
        RefreshIcons();
    }

    // Expose the player's current inventory to the UI
    public ItemData[] GetItems()
    {
        return player != null ? player.GetItems() : new ItemData[_fixedInventorySize];
    }

    // Rebuilds the icon list on the panel
    public void RefreshIcons()
    {
        if (itemPanel == null || itemIconPrefab == null || player == null)
            return;

        var templateTransform = itemIconPrefab.transform;
        RemoveOldIcons(templateTransform);

        ItemData[] currentItems = player.GetItems();

        for (int i = 0; i < currentItems.Length; i++)
        {
            ItemData item = currentItems[i];
            Image iconInstance = Instantiate(itemIconPrefab, itemPanel.transform);
            iconInstance.sprite = item != null ? item.icon : null;

            RectTransform rt = iconInstance.rectTransform;
            int col = i % itemsPerRow;
            int row = i / itemsPerRow;
            rt.anchoredPosition = startOffset + new Vector2(col * spacing.x, row * spacing.y);

            iconInstance.gameObject.SetActive(true);
        }

        
        templateTransform.gameObject.SetActive(false);
    }
    
    private void RemoveOldIcons(Transform templateTransform)
    {
        foreach (Transform child in itemPanel.transform)
        {
            if (child.CompareTag("Background"))
            {
                continue;
            }
            if (templateTransform != null && child == templateTransform)
            {
                child.gameObject.SetActive(false);
                continue;
            }
            Destroy(child.gameObject);
        }
    }
    
    public void UpdateSelectedSlot(int selectedSlot)
    {
        // TODO: implement this
    }
}

}