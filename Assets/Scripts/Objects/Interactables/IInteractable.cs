using UnityEngine;

namespace Objects.Interactables
{
    public interface IInteractable
    {
        string InteractionPrompt { get; set;}
        
        void Interact(Player interactor);
    }

}

