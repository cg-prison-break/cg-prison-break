using UnityEngine;

public class SuspiciousArea : MonoBehaviour
{
    [SerializeField] private bool IsCameraArea;
    [SerializeField] private ItemData Flashlight;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        var player = other.GetComponentInParent<Player>();
        bool playerHasFlashlight = Flashlight is not null ? player.HasItem(Flashlight) : false;

        if ((IsCameraArea && !playerHasFlashlight) || !IsCameraArea)
        {
            NPCEventManager.NotifyNPCsAboutSuspiciousAction(transform.position, true);
        }
    }
}
