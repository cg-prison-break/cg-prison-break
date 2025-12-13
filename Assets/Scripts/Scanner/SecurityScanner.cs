using System.Collections.Generic;
using UnityEngine;

namespace Scanner
{
    public class SecurityScanner : MonoBehaviour
    {
        [SerializeField] private List<ItemData> illegalItems;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            var player = other.GetComponent<Player>();
            if (!player.HasOneOf(illegalItems)) return;
            var audioSource = GetComponent<AudioSource>();
            audioSource.Play();
            NPCEventManager.NotifyNPCsAboutSuspiciousAction(player.transform.position);
        }
    }
}