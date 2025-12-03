using UnityEngine;

public abstract class NPCState
{
    public abstract void EnterState(NPC npc);
    public abstract void UpdateState(NPC npc);
    public abstract void ExitState(NPC npc);
}
