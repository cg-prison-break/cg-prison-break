using Objects.Interactables.Items;
using UnityEngine;

public class DynamiteItem : MonoBehaviour, IInteractableItem
{
    [SerializeField] private ItemData _itemData;
    public AudioClip pickupSoundClip;
    
    public ItemData itemData
    {
        get { return _itemData; }
        set { _itemData = value; }
    }

    public string InteractionPrompt
    {
        get => "Press F to pick up dynamite.";
        set => InteractionPrompt = value;
    }
    
    public void Interact(Player interactor)
    {   
        var pickedUp = interactor.AddItem(itemData);
        Debug.Log($"Item Name: {_itemData.itemName}");
        interactor.GetComponents<AudioSource>()[0].PlayOneShot(pickupSoundClip);
        NPCEventManager.NotifyNPCsAboutSuspiciousAction(interactor.transform.position);
        if (pickedUp) Destroy(gameObject);
    }
}