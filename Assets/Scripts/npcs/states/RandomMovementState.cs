using UnityEngine;

public class RandomMovementState : NPCState
{
    protected override Color? StateHintColor => Color.green;

    private readonly float minDistanceToNextLocation = 4f;
    private readonly float nextLocationRadius = 15f;

    public override void EnterState(NPC npc)
    {
        if (NavMeshUtils.TryFindValidNavMeshPosition(npc.transform.position, nextLocationRadius, minDistanceToNextLocation, out var nextTarget))
        {
            npc.Movement.TryMoveToDestination(nextTarget);
            npc.Movement.StartWalking();
        }

        UpdateStateHint(npc);
    }

    public override void ExitState(NPC npc)
    {
        npc.Movement.StopWalking();
        npc.Movement.SetSpeed(npc.Movement.defaultSpeed);
    }

    public override void UpdateState(NPC npc)
    {
        if (!npc.Movement.PathIsValid() || npc.Movement.HasReachedDestination())
        {
            if (NavMeshUtils.TryFindValidNavMeshPosition(npc.transform.position, nextLocationRadius, minDistanceToNextLocation, out var nextTarget))
            {
                npc.Movement.TryMoveToDestination(nextTarget);
            }
        }
        else if (npc.Movement.IsStuck())
        {
            npc.Movement.StopWalking();
            Vector3 newTarget = npc.transform.position - npc.transform.forward;

            if (NavMeshUtils.TryFindValidNavMeshPosition(newTarget, nextLocationRadius, minDistanceToNextLocation, out var nextTarget))
            {
                npc.Movement.TryMoveToDestination(nextTarget);
                npc.Movement.StartWalking();
            }
        }
    }
}
