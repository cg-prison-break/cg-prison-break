using Objects.Interactables;
using UnityEngine;

namespace Prefabs.Interactions.screwdriver
{
    public class WindowScrewHandler : MonoBehaviour, IInteractable
    {
        private int amountScrews = 4;
        public GameObject windowWithoutGridWindow;
        
        public void notifyAboutUnscrewAction()
        {
            amountScrews--;

            if (amountScrews == 0)
            {
                HandleWindowUnscrewed();
            }
        }
        
        private void HandleWindowUnscrewed()
        {
            gameObject.layer = LayerMask.NameToLayer("Interactable");
        }

        public void Interact(Player interactor)
        {
            Instantiate(windowWithoutGridWindow, transform.position, transform.rotation);
            NPCEventManager.NotifyNPCsAboutSuspiciousAction(interactor.transform.position);
            GameTelemetryLogger.LogTelemetryEvent(new SuspiciousEventTriggeredData("WindowUnscrewed"));
            Destroy(gameObject);
        }
        
        public string InteractionPrompt
        {
            get => "Drücke F, um Fenster zu entfernen.";
            set => InteractionPrompt = value;   
        }
    }
}
