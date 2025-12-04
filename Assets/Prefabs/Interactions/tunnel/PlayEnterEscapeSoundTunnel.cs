using UnityEngine;

namespace Prefabs.Interactions.tunnel
{
    public class PlayEnterEscapeSoundTunnel : MonoBehaviour
    {
        public AudioSource audioSource;
        public AudioClip enterSound;
        public AudioClip escapeSound;
        public TunnelState tunnelState;
    
        public void PlayInteractionSound()
        {
            audioSource.PlayOneShot(tunnelState.GetInTunnel() ? escapeSound : enterSound);
        }
    }
}
