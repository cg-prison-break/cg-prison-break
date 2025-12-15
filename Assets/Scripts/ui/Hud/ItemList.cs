using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemList : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private GameObject itemPanel;
    [SerializeField] private Image itemIconPrefab;
    [SerializeField] private Vector2 startOffset = new Vector2(30f, -30f);
    [SerializeField] private Vector2 spacing = new Vector2(55f, -55f);
    [SerializeField] private int itemsPerRow = 4;

    private void OnEnable()
    {
        RefreshIcons();
    }

    // Expose the player's current inventory to the UI
    public List<ItemData> GetItems()
    {
        return player != null ? player.GetItems() : new List<ItemData>();
    }

    // Rebuilds the icon list on the panel
    public void RefreshIcons()
    {
        if (itemPanel == null)
        {
            return;
        }

        Transform templateTransform = itemIconPrefab != null ? itemIconPrefab.transform : null;
        foreach (Transform child in itemPanel.transform)
        {
            if (templateTransform != null && child == templateTransform)
            {
                child.gameObject.SetActive(false);
                continue;
            }
            Destroy(child.gameObject);
        }

        if (player == null /*|| itemIconPrefab == null*/)
        {
            return;
        }

        List<ItemData> currentItems = player.GetItems();

        for (int i = 0; i < currentItems.Count; i++)
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

        if (templateTransform != null)
        {
            templateTransform.gameObject.SetActive(false);
        }
    }
}
