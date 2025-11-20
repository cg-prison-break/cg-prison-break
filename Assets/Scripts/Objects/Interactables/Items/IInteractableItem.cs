namespace Objects.Interactables.Items
{
    public interface IInteractableItem: IInteractable
    {
        ItemData itemData {get; set;}
    }
}