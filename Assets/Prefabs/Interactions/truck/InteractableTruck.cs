using System.Collections;
using System.Collections.Generic;
using Objects.Interactables;
using UnityEngine;

namespace Prefabs.Interactions.truck
{
    public class InteractableTruck : MonoBehaviour, IInteractableConnected
    {
        public Transform newPlayerTransform;
        public GameObject newTruckAtOtherPosition;
        public Transform waitingPlayerPosition; 
        
        [SerializeField] private List<ItemData> _connectedItems;
    
        public List<ItemData> ConnectedItems => _connectedItems;
        
        public string InteractionPrompt
        {
            get => "Press F escape with truck.";
            set => InteractionPrompt = value;
        }

        public void Interact(Player interactor)
        {
            if (!interactor.HasAll(_connectedItems)) return;
            
            interactor.RemoveItem(_connectedItems[0]);
            NPCEventManager.NotifyNPCsAboutSuspiciousAction(interactor.transform.position);
            
            var characterController = interactor.GetComponent<CharacterController>();
            characterController.enabled = false;
            
            interactor.transform.position = waitingPlayerPosition.position;
            interactor.transform.rotation = waitingPlayerPosition.rotation;
        
            var audioSourceTruck = gameObject.GetComponent<AudioSource>();
            audioSourceTruck.Play();
            StartCoroutine(WaitForEndOfAudio(interactor, characterController));
        }

        private IEnumerator WaitForEndOfAudio(Player interactor, CharacterController characterController)
        {
            yield return new WaitForSeconds(7f);
        
            interactor.transform.position = newPlayerTransform.position;
            interactor.transform.rotation = newPlayerTransform.rotation;
            newTruckAtOtherPosition.SetActive(true);
            
            characterController.enabled = true;
            NPCEventManager.NotifyNPCsAboutSuspiciousAction(interactor.transform.position);
            Destroy(gameObject);
        }
    }
}
