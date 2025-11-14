using UnityEngine;

namespace Objects.Interactables
{
    public interface IInteractable
    {
        string InteractionPrompt { get; }
        
        void Interact(Player interactor);
    }

}

