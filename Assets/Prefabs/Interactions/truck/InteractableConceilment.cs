using Objects.Interactables.Items;
using UnityEngine;

namespace Prefabs.Interactions.truck
{
    public class InteractableConceilment : MonoBehaviour, IInteractableItem
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
            get => "Press F to pick up conceilment.";
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
}