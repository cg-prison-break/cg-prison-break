
public class PauseState : NPCState
{
    public override void EnterState(NPC npc)
    {
        npc.navMeshAgent.isStopped = true;
    }

    public override void ExitState(NPC npc)
    {
        npc.navMeshAgent.isStopped = false;
    }

    public override void UpdateState(NPC npc) { }
}
