using Objects.Interactables;
using UnityEngine;

namespace Prefabs.Interactions
{
    public class ClimbOntoFence : MonoBehaviour, IInteractable
    {
        public string InteractionPrompt { get; set; } = "Press F to climb onto fence.";
        
        public void Interact(Player interactor)
        {
            var cc = interactor.GetComponent<CharacterController>();
            SetCharacterController(cc, false);

            var climbingPoint = gameObject.GetComponentInChildren<ClimbingPoint>().transform.position;
            interactor.transform.position = climbingPoint;
            
            SetCharacterController(cc, true);
            Debug.Log("Climbed onto fence.");
        }
        
        private static void SetCharacterController(CharacterController cc, bool enabled)
        {
            if (cc != null) cc.enabled = enabled;
        }
    }
}
