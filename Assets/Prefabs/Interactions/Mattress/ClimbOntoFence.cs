using Objects.Interactables;
using Prefabs.Interactions.Mattress;
using UnityEngine;

namespace Prefabs.Interactions
{
    public class ClimbOntoFence : MonoBehaviour, IInteractable
    {
        public AudioSource metalRattleSource;
        
        public string InteractionPrompt { get; set; } = "Drücke F, um auf den Zaun zu klettern.";
        
        public void Interact(Player interactor)
        {   
            var cc = interactor.GetComponent<CharacterController>();
            SetCharacterController(cc, false);

            var climbingPoint = gameObject.GetComponentInChildren<ClimbingPoint>().transform.position;
            interactor.transform.position = climbingPoint;
            metalRattleSource.Play();
            NPCEventManager.NotifyNPCsAboutSuspiciousAction(interactor.transform.position);
            
            SetCharacterController(cc, true);
            Debug.Log("Climbed onto fence.");
        }
        
        private static void SetCharacterController(CharacterController cc, bool enabled)
        {
            if (cc != null) cc.enabled = enabled;
        }
    }
}
