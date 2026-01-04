using UnityEngine;

public class SuspiciousArea : MonoBehaviour
{
    [SerializeField] private bool CanBeBypassed;
    [SerializeField] private ItemData BypassItem;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        var player = other.GetComponentInParent<Player>();
        bool playerHasBypassItem = BypassItem is not null ? player.HasItem(BypassItem) : false;

        if ((CanBeBypassed && !playerHasBypassItem) || !CanBeBypassed)
        {
            NPCEventManager.NotifyNPCsAboutSuspiciousAction(transform.position, true);
            GameTelemetryLogger.LogTelemetryEvent(new SuspiciousEventTriggeredData("SuspiciousAreaEntered"));
        }
    }
}
