using Objects.Interactables.Items;
using UnityEngine;

public class ShovelItem : MonoBehaviour, IInteractableItem
{
    [Header("Data")]
    [SerializeField] private ItemData _shovelItemData;

    public ItemData itemData
    {
        get { return _shovelItemData; }
        set { _shovelItemData = value; }
    }

    public string InteractionPrompt { get; set; }
    
    public void Interact(Player interactor)
    {
        interactor.AddItem(_shovelItemData);    
        Destroy(gameObject);
    }
}
