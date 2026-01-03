using Objects.Interactables.Items;
using UnityEngine;

namespace Prefabs.Interactions.truck
{
    public class InteractableConceilment : MonoBehaviour, IInteractableItem
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
            if (pickedUp) Destroy(gameObject);
        }
    }
}