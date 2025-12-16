using Objects.Interactables;
using UnityEngine;

namespace Prefabs.Interactions.secret_escape
{
    public class InteractableLaptop : MonoBehaviour, IInteractable
    {
        public GameObject laptopHackingPrefab;
    
        public string InteractionPrompt
        {
            get => "Drücke F, um dich in den Laptop zu hacken und die sofortige Freilassung zu genehmigen.";
            set => InteractionPrompt = value;
        }

        public void Interact(Player interactor)
        {
            Instantiate(laptopHackingPrefab, transform.position, transform.rotation);   
            Destroy(gameObject);
        }
    }
}
