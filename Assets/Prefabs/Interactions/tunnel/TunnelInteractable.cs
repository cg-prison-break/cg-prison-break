using Objects.Interactables;
using UnityEngine;

namespace Prefabs.Interactions.tunnel
{
    public class TunnelInteractable : MonoBehaviour, IInteractable
    {
        public TunnelState tunnelState;
        private bool _manipulatedBox;
        private string _interactionPromptWhenOutOfTunnel = "Drücke F, um den Tunnel zu betreten.";
        private string _interactionPromptWhenInTunnel = "Drücke F, um den Tunnel zu verlassen.";
        public AudioSource audioSource;
        public PlayEnterEscapeSoundTunnel playEnterEscapeSoundTunnel;
        [SerializeField] private GameData gameData;

        public string InteractionPrompt
        {
            get =>
                tunnelState.GetInTunnel()
                    ? _interactionPromptWhenInTunnel
                    : _interactionPromptWhenOutOfTunnel;
            set => InteractionPrompt = value;
        }

        public void Interact(Player interactor)
        {
            NPCEventManager.NotifyNPCsAboutSuspiciousAction(interactor.transform.position);
            GameTelemetryLogger.LogTelemetryEvent(new SuspiciousEventTriggeredData("ClimbedIntoTunnel"));

            var cc = interactor.GetComponent<CharacterController>();
            SetCharacterController(cc, false);

            if (!tunnelState.GetInTunnel())
            {
                HandleTunnelInteraction(interactor, true, "Climbed into tunnel.");
            }
            else
            {
                HandleTunnelInteraction(interactor, false, "Climbed out of tunnel.");
            }
            
            if (!_manipulatedBox)
            {
                AdjustWoodenTunnelPlateWhenFirstEntered();
            }

            SetCharacterController(cc, true);
        }

        private void AdjustWoodenTunnelPlateWhenFirstEntered()
        {
            var woodPlate = gameObject.GetComponentInChildren<WoodenTunnelPlate>().transform;
            woodPlate.transform.rotation = Quaternion.Euler(0, 30, 0);
            woodPlate.transform.position += new Vector3(0, 0, -0.5f);
            audioSource.Play();
            _manipulatedBox = true;
        }

        private void HandleTunnelInteraction(Player interactor, bool inTunnel, string logMessage)
        {
            var enterPoint = tunnelState.GetInTunnel() ? gameObject.GetComponentInChildren<LeavePoint>().transform.position : gameObject.GetComponentInChildren<EnterPoint>().transform.position;
            Debug.Log(enterPoint);

            interactor.transform.position = enterPoint;
            Debug.Log(interactor.transform.position);
            playEnterEscapeSoundTunnel.PlayInteractionSound();

            tunnelState.SetInTunnel(inTunnel);
            Debug.Log("Tunnel state changed to:" + tunnelState.GetInTunnel());
            Debug.Log(logMessage);
        }

        private static void SetCharacterController(CharacterController cc, bool enabled)
        {
            if (cc != null) cc.enabled = enabled;
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