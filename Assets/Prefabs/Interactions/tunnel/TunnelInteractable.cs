using Objects.Interactables;
using UnityEngine;

namespace Prefabs.Interactions.tunnel
{
    public class TunnelInteractable : MonoBehaviour, IInteractable
    {
        public TunnelState tunnelState;
        private bool _manipulatedBox;
        private string _interactionPromptWhenOutOfTunnel = "Press F to climb into tunnel.";
        private string _interactionPromptWhenInTunnel = "Press F to climb out of tunnel.";

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
            _manipulatedBox = true;
        }

        private void HandleTunnelInteraction(Player interactor, bool inTunnel, string logMessage)
        {
            var enterPoint = gameObject.GetComponentInChildren<EnterPoint>().transform.position;
            interactor.transform.position = enterPoint;

            tunnelState.SetInTunnel(inTunnel);
            Debug.Log(logMessage);
        }

        private static void SetCharacterController(CharacterController cc, bool enabled)
        {
            if (cc != null) cc.enabled = enabled;
        }
    }
}