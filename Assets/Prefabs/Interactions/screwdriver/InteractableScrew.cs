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
            get => "Press F to unscrew.";
            set => InteractionPrompt = value;   
        }


        public void Interact(Player interactor)
        {
            if (!interactor.HasAll(_connectedItems)) return;
            if (_isScrewing) return;

            Debug.Log("Screwing...");
            Debug.Log("Screwing...");
            NPCEventManager.NotifyNPCsAboutSuspiciousAction(interactor.transform.position);
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
