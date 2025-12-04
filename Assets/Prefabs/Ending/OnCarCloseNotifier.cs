using UnityEngine;

namespace Prefabs.Ending
{
    public class OnCarCloseNotifier : MonoBehaviour
    {
        public OnCarCloseBehaviour carCloseBehaviour;
    
        public void NotifyWhenClosed()
        {
            carCloseBehaviour.OnCarClose();
        }
    }
}
