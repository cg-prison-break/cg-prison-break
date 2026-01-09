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
        
        [SerializeField] private GameData gameData;


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
                GameTelemetryLogger.LogTelemetryEvent(new SuspiciousEventTriggeredData("FenceCut"));
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
        
        private void FixedUpdate()
        {
            var player = PlayerRegistry.Player;
            // check if the player is near to the object, then set the layer of the object and all of its children to "Interactable"
            if (Vector3.Distance(transform.position, player.transform.position) < gameData.interactableDisplayDistance)
            {
                gameObject.layer =  LayerMask.NameToLayer(GetInteractableLayerName());
                if (!gameData.playWithInteractableShader)
                {
                    // todo: implement logic for making lights on when shader is disabled
                }
            }
            else
            {
                gameObject.layer = LayerMask.NameToLayer("Default");
            }
        }
        
        private string GetInteractableLayerName()
        {
            return gameData.playWithInteractableShader ? "Interactable" : "InteractableNoOutline";
        }
    }
}