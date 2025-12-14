using System.Collections.Generic;
using Objects.Interactables;
using UnityEngine;

namespace Prefabs.Interactions.wall_detonation
{
    public class PlaceDynamiteInteractable : MonoBehaviour, IInteractableConnected
    {
        [SerializeField] private List<ItemData> _connectedItems;
        public List<ItemData> ConnectedItems => _connectedItems;
        public AudioClip placeDynamiteSoundClip;
        public GameObject parentWall;
        
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
                    interactionPrompt = "Drücke F, um Wand zu sprengen.";
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

            var gameObjectDynamite = Instantiate(_connectedItems[0].prefab, transform.position, transform.rotation);
            var burningDynamite = gameObjectDynamite.GetComponent<BurningDynamite>();
            burningDynamite.SetParentWall(parentWall);
            interactor.RemoveItem(_connectedItems[0]);
            interactor.GetComponents<AudioSource>()[0].PlayOneShot(placeDynamiteSoundClip);
            NPCEventManager.NotifyNPCsAboutSuspiciousAction(interactor.transform.position);
        }
    }
}
