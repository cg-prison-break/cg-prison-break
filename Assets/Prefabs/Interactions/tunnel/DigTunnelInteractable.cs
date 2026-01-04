using Objects.Interactables;
using UnityEngine;

namespace Prefabs.Interactions.tunnel
{
    public class DigTunnelInteractable : MonoBehaviour, IInteractable
    {
        public TunnelState tunnelState;
        private bool _manipulatedBox;
        private string _interactionPromptWhenOutOfTunnel = "Drücke F, um den Tunnel zu betreten.";
        private string _interactionPromptWhenInTunnel = "Drücke F, um den Tunnel zu verlassen.";
        public AudioSource audioSource;
        public AudioClip enterSound;
        public AudioClip escapeSound;
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
            var cc = interactor.GetComponent<CharacterController>();
            SetCharacterController(cc, false);

            if (!tunnelState.GetInTunnel())
            {
                HandleTunnelInteraction(interactor, true, "Climbed into tunnel.");
                audioSource.PlayOneShot(enterSound);
            }
            else
            {
                HandleTunnelInteraction(interactor, false, "Climbed out of tunnel.");
                audioSource.PlayOneShot(escapeSound);
            }

            SetCharacterController(cc, true);
        }

        private void HandleTunnelInteraction(Player interactor, bool inTunnel, string logMessage)
        {
            Debug.Log("Tunnel interaction triggered.");
            var enterPoint = tunnelState.GetInTunnel() ? gameObject.GetComponentInChildren<LeavePoint>().transform.position : gameObject.GetComponentInChildren<EnterPoint>().transform.position;
            Debug.Log(enterPoint);

            interactor.transform.position = enterPoint;
            Debug.Log(interactor.transform.position);

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