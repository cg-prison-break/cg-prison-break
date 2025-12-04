using Objects.Interactables;
using Prefabs.Interactions.tunnel;
using UnityEngine;

namespace Prefabs.Interactions.screwdriver
{
    public class WindowInteractable : MonoBehaviour, IInteractable
    {
        public TunnelState tunnelState;
        
        public string InteractionPrompt
        {
            get => "Press F to climb through window.";
            set => InteractionPrompt = value;   
        }

        public void Interact(Player interactor)
        {
            var characterController = interactor.GetComponent<CharacterController>();
            characterController.enabled = false;

            NPCEventManager.NotifyNPCsAboutSuspiciousAction(interactor.transform.position);
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
    }
}
