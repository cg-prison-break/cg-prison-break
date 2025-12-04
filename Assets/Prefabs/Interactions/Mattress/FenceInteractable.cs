using System.Collections.Generic;
using Objects.Interactables;
using UnityEngine;

namespace Prefabs.Interactions.Mattress
{
    public class FenceInteractable : MonoBehaviour, IInteractableConnected
    {
        [SerializeField] private List<ItemData> _connectedItems;
        [SerializeField] private ItemData wireCutterItemData;
        public GameObject fenceWithMattressPrefab;
        public GameObject fenceWithWholePrefab;
        public GameObject parent;
        public GameObject animatedWireCutter;
        public AudioClip cutWireClip;
        public List<ItemData> ConnectedItems => _connectedItems;


        public string InteractionPrompt
        {
            get => "Press F to place mattress on fence.";
            set => InteractionPrompt = value;
        }

        public void Interact(Player interactor)
        {
            if (interactor.HasItem(wireCutterItemData))
            {
                NPCEventManager.NotifyNPCsAboutSuspiciousAction(interactor.transform.position);
                var audioSourceFenceRattle = interactor.GetComponents<AudioSource>()[0];
                audioSourceFenceRattle.PlayOneShot(cutWireClip);
                animatedWireCutter.SetActive(true);
            }
            else if (interactor.HasAll(_connectedItems))
            {
                foreach (var item in ConnectedItems)
                {
                    interactor.RemoveItem(item);
                }

                var gameObjectFenceWithMattress = Instantiate(fenceWithMattressPrefab, parent.transform.position,
                    parent.transform.rotation);
                var audioSourcePlaceMattress = gameObjectFenceWithMattress.GetComponents<AudioSource>()[0];
                var audioSourceFenceRattle = gameObjectFenceWithMattress.GetComponents<AudioSource>()[1];
                NPCEventManager.NotifyNPCsAboutSuspiciousAction(interactor.transform.position);
                audioSourceFenceRattle.Play();
                audioSourcePlaceMattress.Play();
                Destroy(parent);
            }
        }

        public void OnAnimationFinished()
        {
            Instantiate(fenceWithWholePrefab, parent.transform.position, parent.transform.rotation);
            Destroy(parent);
        }
    }
}