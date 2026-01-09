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
        public GameObject animatedDynamite;
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

            foreach (var item in ConnectedItems)
            {
                GameTelemetryLogger.LogTelemetryEvent(new ItemUsedData(item.itemName));
            }

            var gameObjectDynamite = Instantiate(animatedDynamite, transform.position, transform.rotation);
            var burningDynamite = gameObjectDynamite.GetComponent<BurningDynamite>();
            burningDynamite.SetParentWall(parentWall);
            interactor.RemoveItem(_connectedItems[0]);
            interactor.GetComponents<AudioSource>()[0].PlayOneShot(placeDynamiteSoundClip);
            NPCEventManager.NotifyNPCsAboutSuspiciousAction(interactor.transform.position);
            GameTelemetryLogger.LogTelemetryEvent(new SuspiciousEventTriggeredData("DynamitePlaced"));
        }
        
        private void FixedUpdate()
        {
            var player = PlayerRegistry.Player;
            // check if the player is near to the object, then set the layer of the object and all of its children to "Interactable"
            if (Vector3.Distance(transform.position, player.transform.position) < gameData.interactableDisplayDistance)
            {
                SetLayerRecursively(gameObject, LayerMask.NameToLayer(GetInteractableLayerName()));
                if (!gameData.playWithInteractableShader)
                {
                    // todo: implement logic for making lights on when shader is disabled
                }
            }
            else
            {
                SetLayerRecursively(gameObject, LayerMask.NameToLayer("Default"));
            }
        }
        
        private void SetLayerRecursively(GameObject obj, int layer)
        {
            obj.layer = layer;
            foreach (Transform child in obj.transform)
            {
                SetLayerRecursively(child.gameObject, layer);
            }
        }
        
        private string GetInteractableLayerName()
        {
            return gameData.playWithInteractableShader ? "Interactable" : "InteractableNoOutline";
        }
    }
}
