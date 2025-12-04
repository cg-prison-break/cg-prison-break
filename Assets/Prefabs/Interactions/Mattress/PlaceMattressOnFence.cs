using System.Collections.Generic;
using Objects.Interactables;
using UnityEngine;

namespace Prefabs.Interactions.Mattress
{
    public class PlaceMattressOnFence : MonoBehaviour, IInteractableConnected
    {
        [SerializeField] private List<ItemData> _connectedItems;
        public GameObject fenceWithMattressPrefab;
        public GameObject parent;
        public List<ItemData> ConnectedItems => _connectedItems;


        public string InteractionPrompt
        {
            get => "Press F to place mattress on fence.";
            set => InteractionPrompt = value;
        }

        public void Interact(Player interactor)
        {
            if (!interactor.HasAll(_connectedItems)) return;

            foreach (var item in ConnectedItems)
            {
                interactor.RemoveItem(item);
            }
            var gameObjectFenceWithMattress = Instantiate(fenceWithMattressPrefab, parent.transform.position, parent.transform.rotation);
            var audioSourcePlaceMattress = gameObjectFenceWithMattress.GetComponents<AudioSource>()[0];
            var audioSourceFenceRattle = gameObjectFenceWithMattress.GetComponents<AudioSource>()[1];
            NPCEventManager.NotifyNPCsAboutSuspiciousAction(interactor.transform.position);
            audioSourceFenceRattle.Play();
            audioSourcePlaceMattress.Play();
            Destroy(parent);
        }
    }
}