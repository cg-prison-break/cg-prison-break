using UnityEngine;

public class SuspiciousArea : MonoBehaviour
{
    [SerializeField] private bool CanBeBypassed;
    [SerializeField] private ItemData BypassItem;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip alertSound;

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
            if (audioSource != null && alertSound != null)
            {
                audioSource.PlayOneShot(alertSound);
            }
            NPCEventManager.NotifyNPCsAboutSuspiciousAction(player.transform.position, true);
            GameTelemetryLogger.LogTelemetryEvent(new SuspiciousEventTriggeredData("SuspiciousAreaEntered"));
        } else
        {
            GameTelemetryLogger.LogTelemetryEvent(new ItemUsedData(BypassItem.itemName));
        }
    }
}
