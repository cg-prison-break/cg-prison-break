using Objects.Interactables.Items;
using UnityEngine;

public class DynamiteItem : MonoBehaviour, IInteractableItem
{
    [SerializeField] private ItemData _itemData;
    
    public ItemData itemData
    {
        get { return _itemData; }
        set { _itemData = value; }
    }

    public string InteractionPrompt
    {
        get => $"Drücke F, um \"{_itemData.itemName}\" aufzunehmen.";
        set => InteractionPrompt = value;
    }
    
    public void Interact(Player interactor)
    {   
        var pickedUp = interactor.AddItem(itemData);
        Debug.Log($"Item Name: {_itemData.itemName}");
        NPCEventManager.NotifyNPCsAboutSuspiciousAction(interactor.transform.position);
        if (pickedUp) Destroy(gameObject);
    }
}