using UnityEngine;

namespace Prefabs.Interactions.tunnel
{
    public class DigInteractableEventForwarder : MonoBehaviour
    {
        public DigInteractable digTarget;
    
        public void OnDigAnimationFinished()
        {
            digTarget.OnDigAnimationFinished();
        }
    }
}
