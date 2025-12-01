using Objects.Interactables;
using UnityEngine;
using UnityEngine.Serialization;

namespace Prefabs.Interactions.tunnel
{
    public class DigInteractable : MonoBehaviour, IInteractableConnected
    {
        public GameObject parent;
        public Animator animator;
        public GameObject shovel;
        
        [SerializeField] private ItemData _shovelItem;

        public ItemData ConnectedItem
        {
            get { return _shovelItem; }
            set { _shovelItem = value; }
        }

        private bool isDigging = false;


        public string InteractionPrompt
        {
            get => "Press F to dig.";
            set => InteractionPrompt = value;
        }


        public void Interact(Player interactor)
        {
            if (isDigging) return;
            if (!interactor.HasItem(_shovelItem))
                return;
            NPCEventManager.NotifyNPCsAboutSuspiciousAction(interactor.transform.position);
            isDigging = true;
            shovel.SetActive(true);
        }

        public void OnDigAnimationFinished()
        {
            Destroy(parent);
        }

        public ItemData shovelItem { get; }
    }
}
