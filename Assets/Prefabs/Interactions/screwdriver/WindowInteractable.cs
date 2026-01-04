using Objects.Interactables;
using Prefabs.Interactions.tunnel;
using UnityEngine;

namespace Prefabs.Interactions.screwdriver
{
    public class WindowInteractable : MonoBehaviour, IInteractable
    {
        public TunnelState tunnelState;
        [SerializeField] private GameData gameData;
        
        public string InteractionPrompt
        {
            get => "Drücke F, um durch das Fenster zu klettern.";
            set => InteractionPrompt = value;   
        }

        public void Interact(Player interactor)
        {
            var characterController = interactor.GetComponent<CharacterController>();
            characterController.enabled = false;

            NPCEventManager.NotifyNPCsAboutSuspiciousAction(interactor.transform.position);
            GameTelemetryLogger.LogTelemetryEvent(new SuspiciousEventTriggeredData("Window"));

            if (tunnelState.GetInTunnel())
            {
                HandleEnterCell(interactor);
            }
            else
            {
                HandleLeaveCell(interactor);
            }

            characterController.enabled = true;
        }

        private void HandleLeaveCell(Player interactor)
        {
            var leavePoint = gameObject.GetComponentInChildren<LeavePoint>();
            interactor.transform.position = leavePoint.transform.position;
            var audioSourceLeavePoint = leavePoint.gameObject.GetComponent<AudioSource>();
            audioSourceLeavePoint.Play();
            tunnelState.SetInTunnel(true);
        }

        private void HandleEnterCell(Player interactor)
        {
            var enterPoint = gameObject.GetComponentInChildren<EnterPoint>();
            interactor.transform.position = enterPoint.transform.position;
            var audioSourceEnterPoint = enterPoint.gameObject.GetComponent<AudioSource>();
            audioSourceEnterPoint.Play();
            tunnelState.SetInTunnel(false);
        }
        
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
