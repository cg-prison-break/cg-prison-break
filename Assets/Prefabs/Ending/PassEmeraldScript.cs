using UnityEngine;

public class PassEmeraldScript : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        gameObject.GetComponent<Collider>().isTrigger = false;
    }
}
