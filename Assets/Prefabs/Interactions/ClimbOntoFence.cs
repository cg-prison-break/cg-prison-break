using System.Collections;
using Objects.Interactables;
using UnityEngine;

namespace Prefabs.Interactions
{
    public class ClimbOntoFence : MonoBehaviour, IInteractable
    {
        public string InteractionPrompt { get; set; } = "Press F to climb onto fence.";
        
        public void Interact(Player interactor)
        {
            // CharacterController vorübergehend deaktivieren
            var cc = interactor.GetComponent<CharacterController>();
            if (cc != null) cc.enabled = false;

            // Teleport
            var climbingPoint = gameObject.GetComponentInChildren<ClimbingPoint>().transform.position;
            interactor.transform.position = climbingPoint;

            // CharacterController wieder aktivieren
            if (cc != null) cc.enabled = true;

            Debug.Log("called climb");
        }
    }
}
