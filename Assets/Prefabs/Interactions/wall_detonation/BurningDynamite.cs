using System.Collections;
using UnityEngine;

namespace Prefabs.Interactions.wall_detonation
{
    public class BurningDynamite : MonoBehaviour
    {
        public AudioSource audioSourceDynamite;
        private GameObject _parentWall;
        public GameObject sparklingParticles;
        public GameObject explosion;

        private void Start()
        {
            StartCoroutine(Burn());
        }
    
        public void SetParentWall(GameObject parentWall)
        {
            _parentWall = parentWall;
        }

        private IEnumerator Burn()
        {
            yield return new WaitForSeconds(0.5f);
            audioSourceDynamite.Play();
            yield return new WaitForSeconds(1.73f);
            Destroy(sparklingParticles);
            var explosionObject = Instantiate(explosion, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(1.3f);
            Explode(explosionObject);
        }

        private void Explode(GameObject explosionObject)
        {
            NPCEventManager.NotifyNPCsAboutSuspiciousAction(transform.position);
            Destroy(_parentWall);
            Destroy(explosionObject);
            Destroy(gameObject);
        }
    }
}
