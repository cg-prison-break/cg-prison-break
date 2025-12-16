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
                    interactionPrompt = "Drücke F, um dich mit Truck rauszuschmuggeln.";
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
