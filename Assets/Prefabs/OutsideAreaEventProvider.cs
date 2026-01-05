using UnityEngine;

public class OutsideAreaEventProvider : MonoBehaviour
{
    private bool eventTriggered = false;
    
    public void MakeAllGuardsAlerted()
    {
        if (eventTriggered) return;
        eventTriggered = true;
        NPCEventManager.MakeAllPrisonGuardsAlwaysSuspcious();
    }
    
    public bool EventTriggered => eventTriggered;
}
