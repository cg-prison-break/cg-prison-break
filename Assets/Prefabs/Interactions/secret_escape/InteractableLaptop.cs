using Objects.Interactables;
using UnityEngine;

namespace Prefabs.Interactions.secret_escape
{
    public class InteractableLaptop : MonoBehaviour, IInteractable
    {
        public GameObject laptopHackingPrefab;
    
        public string InteractionPrompt
        {
            get => "Press F to hack into laptop and apply for immediate release from prison.";
            set => InteractionPrompt = value;
        }

        public void Interact(Player interactor)
        {
            Instantiate(laptopHackingPrefab, transform.position, transform.rotation);   
            Destroy(gameObject);
        }
    }
}
