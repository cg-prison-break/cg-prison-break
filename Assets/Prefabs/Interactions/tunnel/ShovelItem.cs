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

    public string InteractionPrompt
    {
        get => $"Drücke F, um \"{_shovelItemData.itemName}\" aufzunehmen.";
        set => InteractionPrompt = value;   
    }
    
    public void Interact(Player interactor)
    {
        interactor.AddItem(_shovelItemData);    
        interactor.GetComponents<AudioSource>()[0].PlayOneShot(pickupSoundClip);
        Destroy(gameObject);
    }
}
