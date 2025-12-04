using UnityEngine;

namespace Prefabs.Interactions.tunnel
{
    public class DiggingParticleActivator : MonoBehaviour
    {
        public GameObject diggingParticle;
        
        public void SetActive()
        {
            diggingParticle.SetActive(true);
        }
    }
}
