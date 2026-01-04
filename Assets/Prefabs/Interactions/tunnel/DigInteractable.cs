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
        
        [SerializeField] private GameData gameData;

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
            if (!interactor.HasAll(_connectedItems)) return;

            foreach (var item in ConnectedItems)
            {
                GameTelemetryLogger.LogTelemetryEvent(new ItemUsedData(item.itemName));
            }

            NPCEventManager.NotifyNPCsAboutSuspiciousAction(interactor.transform.position);
            GameTelemetryLogger.LogTelemetryEvent(new SuspiciousEventTriggeredData("TunnelDigged"));
            _isDigging = true;
            shovel.SetActive(true);
        }

        public void OnDigAnimationFinished()
        {
            Destroy(parent);
        }
        
        private void FixedUpdate()
        {
            var player = PlayerRegistry.Player;
            // check if the player is near to the object, then set the layer of the object and all of its children to "Interactable"
            if (Vector3.Distance(transform.position, player.transform.position) < 5.5f)
            {
                SetLayerRecursively(gameObject, LayerMask.NameToLayer("Interactable"));
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
    }
}
