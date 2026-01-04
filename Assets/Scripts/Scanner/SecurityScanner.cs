using System.Collections.Generic;
using UnityEngine;

namespace Scanner
{
    public class SecurityScanner : MonoBehaviour
    {
        [SerializeField] private List<ItemData> illegalItems;
        [SerializeField] private ItemData byPassItem;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            var player = other.GetComponent<Player>();
            if (!player.HasOneOf(illegalItems)) return;
            if (player.HasItem(byPassItem))
            {
                GameTelemetryLogger.LogTelemetryEvent(new ItemUsedData(byPassItem.itemName));
                return;
            }
            var audioSource = GetComponent<AudioSource>();
            audioSource.Play();
            var susPoints = GetComponentsInChildren<SusPoint>();
            var randomSusPoint = susPoints[Random.Range(0, susPoints.Length)];
            NPCEventManager.NotifyNPCsAboutSuspiciousAction(randomSusPoint.transform.position);
            GameTelemetryLogger.LogTelemetryEvent(new SuspiciousEventTriggeredData("SecurityScanner"));
        }
    }
}