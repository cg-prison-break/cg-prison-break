using System.Collections.Generic;

namespace Objects.Interactables
{
    public interface IInteractableConnected: IInteractable
    {
        List<ItemData> ConnectedItem {get;}
    }
}