using Objects.Interactables;
using UnityEngine;

namespace Prefabs.StartAnimation
{
    public class SpreadSheetMock : MonoBehaviour, IInteractable
    {
        public string InteractionPrompt
        {
            get => "Drücke M, um \"Hinweise\" zu öffnen";
            set => InteractionPrompt = value;   
        }
        
        public void Interact(Player interactor)
        {
            // intentionally left empty
        }
    }
}
