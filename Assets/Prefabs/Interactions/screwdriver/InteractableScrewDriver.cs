using Objects.Interactables.Items;
using UnityEngine;

public class InteractableScrewDriver : MonoBehaviour, IInteractableItem
{
    [Header("Data")]
    [SerializeField] private ItemData _itemData;
    public AudioClip pickupSoundClip;

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
        interactor.AddItem(itemData);
        interactor.GetComponents<AudioSource>()[0].PlayOneShot(pickupSoundClip);
        NPCEventManager.NotifyNPCsAboutSuspiciousAction(interactor.transform.position);
        Destroy(gameObject);
    }
}
