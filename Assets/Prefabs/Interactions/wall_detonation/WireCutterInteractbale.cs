using Objects.Interactables.Items;
using UnityEngine;

public class WireCutterInteractbale : MonoBehaviour, IInteractableItem
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
        get => "Click F to pick up wirecutter!";
        set => InteractionPrompt = value;   
    }

    public void Interact(Player player)
    {
        var pickedUp = player.AddItem(itemData);
        player.GetComponents<AudioSource>()[0].PlayOneShot(pickupSoundClip);
        if (pickedUp) Destroy(gameObject);
    }
    
}