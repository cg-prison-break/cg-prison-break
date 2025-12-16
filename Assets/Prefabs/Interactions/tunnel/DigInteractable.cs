using System.Collections.Generic;
using Objects.Interactables;
using UnityEngine;

namespace Prefabs.Interactions.tunnel
{
    public class DigInteractable : MonoBehaviour, IInteractableConnected
    {
        public GameObject parent;
        public Animator animator;
        public GameObject shovel;
        
        [SerializeField] private List<ItemData> _connectedItems;

        public List<ItemData> ConnectedItems => _connectedItems;

        private bool _isDigging;

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
                    interactionPrompt = "Drücke F, um zu graben.";
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
            if (_isDigging) return;
            if (!interactor.HasAll(_connectedItems))
                return;
            NPCEventManager.NotifyNPCsAboutSuspiciousAction(interactor.transform.position);
            _isDigging = true;
            shovel.SetActive(true);
        }

        public void OnDigAnimationFinished()
        {
            Destroy(parent);
        }
    }
}
