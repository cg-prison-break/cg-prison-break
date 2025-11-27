using UnityEngine;

namespace Prefabs.Interactions.tunnel
{
    public class TunnelState : MonoBehaviour
    {
        private bool _inTunnel;

        public bool GetInTunnel()
        {
            return _inTunnel;
        }
    
        public void SetInTunnel(bool value)
        {
            _inTunnel = value;
        }
    }
}
