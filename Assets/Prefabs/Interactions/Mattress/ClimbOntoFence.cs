using Objects.Interactables;
using Prefabs.Interactions.Mattress;
using UnityEngine;

namespace Prefabs.Interactions
{
    public class ClimbOntoFence : MonoBehaviour, IInteractable
    {
        public AudioSource metalRattleSource;
        [SerializeField] private GameData gameData;
        
        public string InteractionPrompt { get; set; } = "Drücke F, um auf den Zaun zu klettern.";
        
        public void Interact(Player interactor)
        {   
            var cc = interactor.GetComponent<CharacterController>();
            SetCharacterController(cc, false);

            var climbingPoint = gameObject.GetComponentInChildren<ClimbingPoint>().transform.position;
            interactor.transform.position = climbingPoint;
            metalRattleSource.Play();
            NPCEventManager.NotifyNPCsAboutSuspiciousAction(interactor.transform.position);
            GameTelemetryLogger.LogTelemetryEvent(new SuspiciousEventTriggeredData("FenceClimbed"));

            SetCharacterController(cc, true);
            Debug.Log("Climbed onto fence.");
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
