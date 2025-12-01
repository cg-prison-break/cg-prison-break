using Objects.Interactables;
using UnityEngine;

namespace Prefabs.Interactions
{
    public class PlaceMattressOnFence : MonoBehaviour, IInteractable
    {
        public GameObject fenceWithMattressPrefab;
        public GameObject parent;

        public string InteractionPrompt
        {
            get => "Press F to place mattress on fence.";
            set => InteractionPrompt = value;
        }

        public void Interact(Player interactor)
        {
            Instantiate(fenceWithMattressPrefab, parent.transform.position, parent.transform.rotation);
            Destroy(parent);
        }
    }
}