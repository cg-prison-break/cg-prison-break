using Objects.Interactables.Items;
using UnityEngine;

public class InteractableScrewDriver : MonoBehaviour, IInteractableItem
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
        get => "Press F to pick up screw driver";
        set => InteractionPrompt = value;   
    }

    public void Interact(Player interactor)
    {
        interactor.AddItem(itemData);
        Destroy(gameObject);
    }
}
