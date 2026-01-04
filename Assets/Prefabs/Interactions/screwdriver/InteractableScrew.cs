using System.Collections.Generic;
using Objects.Interactables;
using UnityEngine;

namespace Prefabs.Interactions.screwdriver
{
    public class InteractableScrew : MonoBehaviour, IInteractableConnected
    {
        [SerializeField]private List<ItemData> _connectedItems;
        public Animator animator;
        public GameObject parent;
        public GameObject animatedScrewDriver;
        public WindowScrewHandler windowScrewHandler;
        public AudioSource audioSource;
        private bool _isScrewing;
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
                    interactionPrompt = "Drücke F, um Schraube abzuschrauben.";
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
            if (_isScrewing) return;

            foreach (var item in ConnectedItems)
            {
                GameTelemetryLogger.LogTelemetryEvent(new ItemUsedData(item.itemName));
            }

            Debug.Log("Screwing...");
            Debug.Log("Screwing...");
            NPCEventManager.NotifyNPCsAboutSuspiciousAction(interactor.transform.position);
            GameTelemetryLogger.LogTelemetryEvent(new SuspiciousEventTriggeredData("Screwed"));
            animatedScrewDriver.SetActive(true);
            _isScrewing = true;
            audioSource.Play();
            animator.Play("ScrewAnimation");
        }
    
        public void OnScrewAnimationFinished()
        {
            Debug.Log("Screwing Finished.");
            windowScrewHandler.notifyAboutUnscrewAction();
            Destroy(parent);
        }

        public List<ItemData> ConnectedItems { get => _connectedItems; }
        
        private void FixedUpdate()
        {
            var player = PlayerRegistry.Player;
            // check if the player is near to the object, then set the layer of the object and all of its children to "Interactable"
            if (Vector3.Distance(transform.position, player.transform.position) < 5.5f)
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
