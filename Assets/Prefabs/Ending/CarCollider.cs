using UnityEngine;

namespace Prefabs.Ending
{
    public class CarCollider : MonoBehaviour
    {
        private static readonly int Open = Animator.StringToHash("Open");
        public AudioClip openClip;
        public Animator animator;
        public Player player;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;

            var audioSource = player.gameObject.GetComponents<AudioSource>()[0];
            audioSource.PlayOneShot(openClip);
            
            Debug.Log("Car hit!");
            animator.SetTrigger(Open);
            Destroy(gameObject);
        }
    }
}
