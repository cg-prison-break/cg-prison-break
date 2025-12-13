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
        public GameObject burningDynamite;
        
        public string InteractionPrompt
        {
            get => "Press F to place dynamite on wall.";
            set => InteractionPrompt = value;
        }

        public void Interact(Player interactor)
        {
            if (!interactor.HasAll(_connectedItems)) return;

            var gameObjectDynamite = Instantiate(burningDynamite, transform.position, transform.rotation);
            var burningDynamiteObject = gameObjectDynamite.GetComponent<BurningDynamite>();
            burningDynamiteObject.SetParentWall(parentWall);
            interactor.RemoveItem(_connectedItems[0]);
            interactor.GetComponents<AudioSource>()[0].PlayOneShot(placeDynamiteSoundClip);
            NPCEventManager.NotifyNPCsAboutSuspiciousAction(interactor.transform.position);
        }
    }
}
