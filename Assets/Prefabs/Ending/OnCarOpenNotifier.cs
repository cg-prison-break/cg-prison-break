using UnityEngine;

namespace Prefabs.Ending
{
    public class OnCarOpenNotifier : MonoBehaviour
    {
        public OnCarOpenBehaviour carOpenBehaviour;
    
        public void NotifyWhenOpened()
        {
            carOpenBehaviour.OnCarOpened();
        }
    }
}
