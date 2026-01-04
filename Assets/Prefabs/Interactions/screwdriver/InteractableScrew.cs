using System.Collections.Generic;
using Objects.Interactables;
using UnityEngine;

namespace Prefabs.Interactions.screwdriver
{
    public class InteractableScrew : MonoBehaviour, IInteractableConnected
    {
        [SerializeField]private List<ItemData> _connectedItems;
        public Animator animator;
        public GameObject parent;
        public GameObject animatedScrewDriver;
        public WindowScrewHandler windowScrewHandler;
        public AudioSource audioSource;
        private bool _isScrewing;
    
        public string InteractionPrompt
        {
            get
            {
                var interactionPrompt = "";
                var playerForItemChecking = PlayerRegistry.Player;
                if (playerForItemChecking == null)
                {
                    Debug.LogError("Player was not found.");
                }
                if (playerForItemChecking.HasAll(_connectedItems))
                {
                    interactionPrompt = "Drücke F, um Schraube abzuschrauben.";
                }
                return interactionPrompt;
            }
            set
            {
                // intentionally left empty
            }
        }


        public void Interact(Player interactor)
        {
            if (!interactor.HasAll(_connectedItems)) return;
            if (_isScrewing) return;

            foreach (var item in ConnectedItems)
            {
                GameTelemetryLogger.LogTelemetryEvent(new ItemUsedData(item.itemName));
            }

            Debug.Log("Screwing...");
            Debug.Log("Screwing...");
            NPCEventManager.NotifyNPCsAboutSuspiciousAction(interactor.transform.position);
            GameTelemetryLogger.LogTelemetryEvent(new SuspiciousEventTriggeredData("Screwed"));
            animatedScrewDriver.SetActive(true);
            _isScrewing = true;
            audioSource.Play();
            animator.Play("ScrewAnimation");
        }
    
        public void OnScrewAnimationFinished()
        {
            Debug.Log("Screwing Finished.");
            windowScrewHandler.notifyAboutUnscrewAction();
            Destroy(parent);
        }

        public List<ItemData> ConnectedItems { get => _connectedItems; }
    }
}
