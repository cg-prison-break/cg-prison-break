using Objects.Interactables;
using UnityEngine;

namespace Prefabs.StartAnimation
{
    public class SpreadSheetMock : MonoBehaviour, IInteractable
    {
        [SerializeField] private GameData gameData;
        
        public string InteractionPrompt
        {
            get => "Drücke M, um \"Hinweise\" anzuzeigen. Diese kannst du jederzeit über M aufrufen.";
            set => InteractionPrompt = value;   
        }
        
        public void Interact(Player interactor)
        {
            // intentionally left empty
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
