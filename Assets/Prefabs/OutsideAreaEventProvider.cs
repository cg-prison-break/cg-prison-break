using UnityEngine;

public class OutsideAreaEventProvider : MonoBehaviour
{
    private bool eventTriggered = false;
    
    public void MakeAllGuardsAlerted()
    {
        if (eventTriggered) return;
        eventTriggered = true;
        NPCEventManager.AlertAllPrisonGuards();
    }
    
    public bool EventTriggered => eventTriggered;
}
