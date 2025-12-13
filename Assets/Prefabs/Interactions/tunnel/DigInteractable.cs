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
            get => "Press F to dig.";
            set => InteractionPrompt = value;
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
