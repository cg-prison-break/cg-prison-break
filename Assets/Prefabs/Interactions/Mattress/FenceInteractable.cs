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
            get
            {
                var interactionPrompt = "";
                var playerForItemChecking = PlayerRegistry.Player;
                if (playerForItemChecking == null)
                {
                    Debug.LogError("Player was not found.");
                }
                if (playerForItemChecking.HasItem(wireCutterItemData))
                {
                    interactionPrompt = "Drücke F, um den Zaun durchzuschneiden.";
                }
                else if (playerForItemChecking.HasAll(_connectedItems))
                {
                    interactionPrompt = "Drücke F, Matratze und Seil zu platzieren.";
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
            if (interactor.HasItem(wireCutterItemData))
            {
                NPCEventManager.NotifyNPCsAboutSuspiciousAction(interactor.transform.position);
                GameTelemetryLogger.LogTelemetryEvent(new ItemUsedData(wireCutterItemData.itemName));
                GameTelemetryLogger.LogTelemetryEvent(new SuspiciousEventTriggeredData("FenceCutted"));
                var audioSourceFenceRattle = interactor.GetComponents<AudioSource>()[0];
                audioSourceFenceRattle.PlayOneShot(cutWireClip);
                animatedWireCutter.SetActive(true);
            }
            else if (interactor.HasAll(_connectedItems))
            {
                foreach (var item in ConnectedItems)
                {
                    interactor.RemoveItem(item);
                    GameTelemetryLogger.LogTelemetryEvent(new ItemUsedData(item.itemName));
                }

                var gameObjectFenceWithMattress = Instantiate(fenceWithMattressPrefab, parent.transform.position,
                    parent.transform.rotation);
                var audioSourcePlaceMattress = gameObjectFenceWithMattress.GetComponents<AudioSource>()[0];
                var audioSourceFenceRattle = gameObjectFenceWithMattress.GetComponents<AudioSource>()[1];
                NPCEventManager.NotifyNPCsAboutSuspiciousAction(interactor.transform.position);
                GameTelemetryLogger.LogTelemetryEvent(new SuspiciousEventTriggeredData("FenceMatressRope"));
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