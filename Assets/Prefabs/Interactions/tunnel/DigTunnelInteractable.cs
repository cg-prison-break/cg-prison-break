using Objects.Interactables;
using UnityEngine;

namespace Prefabs.Interactions.tunnel
{
    public class DigTunnelInteractable : MonoBehaviour, IInteractable
    {
        public TunnelState tunnelState;
        private bool _manipulatedBox;
        private string _interactionPromptWhenOutOfTunnel = "Press F to climb into tunnel.";
        private string _interactionPromptWhenInTunnel = "Press F to climb out of tunnel.";
        public AudioSource audioSource;
        public AudioClip enterSound;
        public AudioClip escapeSound;

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
    }
}