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

    public void SetParticleSystemActive()
    {
        GameObject particleObject = transform.GetChild(0).gameObject;
        particleObject.SetActive(true);
    }

    public string InteractionPrompt
    {
        get => "Press F to pick up dynamite.";
        set => InteractionPrompt = value;
    }
    
    public void Interact(Player interactor)
    {   
        bool pickedUp = interactor.AddItem(itemData);
        Debug.Log($"Item Name: {_itemData.itemName}");
        if (pickedUp) Destroy(gameObject);
       
    }
}