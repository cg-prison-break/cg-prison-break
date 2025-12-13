using Prefabs.Interactions.Mattress;
using UnityEngine;

namespace Prefabs.Interactions.wall_detonation
{
    public class WireCutterFinishNotifier : MonoBehaviour
    {
        public FenceInteractable fenceInteractable;
    
        public void Notify()
        {
            fenceInteractable.OnAnimationFinished();
        }
    }
}
