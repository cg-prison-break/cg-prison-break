using Objects.Interactables;
using UnityEngine;

namespace Prefabs.Interactions
{
    public class PlaceMattressOnFence : MonoBehaviour, IInteractable
    {
        public GameObject fenceWithMattressPrefab;

        public string InteractionPrompt
        {
            get => "Press F to place mattress on fence.";
            set => InteractionPrompt = value;
        }

        public void Interact(Player interactor)
        {
            Instantiate(fenceWithMattressPrefab, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}