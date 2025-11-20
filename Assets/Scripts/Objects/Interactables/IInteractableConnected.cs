namespace Objects.Interactables
{
    public interface IInteractableConnected: IInteractable
    {
        ItemData ConnectedItem {get;}
    }
}