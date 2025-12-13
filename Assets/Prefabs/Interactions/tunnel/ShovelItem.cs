using Objects.Interactables.Items;
using UnityEngine;

public class ShovelItem : MonoBehaviour, IInteractableItem
{
    [Header("Data")]
    [SerializeField] private ItemData _shovelItemData;
    public AudioClip pickupSoundClip;

    public ItemData itemData
    {
        get { return _shovelItemData; }
        set { _shovelItemData = value; }
    }

    public string InteractionPrompt { get; set; }
    
    public void Interact(Player interactor)
    {
        var pickedUp = interactor.AddItem(itemData);

        if (!pickedUp) return;
        interactor.GetComponents<AudioSource>()[1].PlayOneShot(pickupSoundClip);
        Destroy(gameObject);
    }
}
