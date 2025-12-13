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
        get => "Press F to pick up screw driver";
        set => InteractionPrompt = value;   
    }

    public void Interact(Player interactor)
    {
        var pickedUp = interactor.AddItem(itemData);

        if (!pickedUp) return;
        interactor.GetComponents<AudioSource>()[1].PlayOneShot(pickupSoundClip);
        Destroy(gameObject);
    }
}
