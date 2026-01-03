using Objects.Interactables.Items;
using UnityEngine;

namespace Prefabs.Interactions.Mattress
{
    public class RopeItemInteractable : MonoBehaviour, IInteractableItem
    {
        [Header("Data")]
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

        public void Interact(Player player)
        {
            var pickedUp = player.AddItem(itemData);
            if (pickedUp) Destroy(gameObject);
        }
    }
}
