using System.Collections;
using Objects.Interactables;
using UnityEngine;

namespace Prefabs.Interactions.truck
{
    public class InteractableTruck : MonoBehaviour, IInteractable
    {
        public Transform newPlayerTransform;
        public GameObject newTruckAtOtherPosition;
        public Transform waitingPlayerPosition; 
    
        public string InteractionPrompt
        {
            get => "Press F escape with truck.";
            set => InteractionPrompt = value;
        }

        public void Interact(Player interactor)
        {
            var characterController = interactor.GetComponent<CharacterController>();
            characterController.enabled = false;
        
            var audioSourceTruck = gameObject.GetComponent<AudioSource>();
            audioSourceTruck.Play();
            StartCoroutine(WaitForEndOfAudio(interactor, characterController));
        }

        private IEnumerator WaitForEndOfAudio(Player interactor, CharacterController characterController)
        {
            yield return new WaitForSeconds(6.5f);
        
            interactor.transform.position = newPlayerTransform.position;
            interactor.transform.rotation = newPlayerTransform.rotation;
            newTruckAtOtherPosition.SetActive(true);
            
            characterController.enabled = true;
            
            Destroy(gameObject);
        }
    }
}
