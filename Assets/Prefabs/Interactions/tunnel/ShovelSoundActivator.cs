using UnityEngine;

namespace Prefabs.Interactions.tunnel
{
    public class ShovelSoundActivator : MonoBehaviour
    {
        public AudioSource audioSource;
    
        public void PlaySound()
        {
            audioSource.Play();
        }
    }
}
