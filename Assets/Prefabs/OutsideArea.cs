using UnityEngine;

public class OutsideArea : MonoBehaviour
{
    [SerializeField] private OutsideAreaEventProvider eventProvider;

    private void OnTriggerEnter(Collider other)
    {
        if (eventProvider.EventTriggered) return;
        eventProvider.MakeAllGuardsAlerted();
    }
}
