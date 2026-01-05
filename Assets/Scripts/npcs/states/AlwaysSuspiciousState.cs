using UnityEngine;

public class AlwaysSuspiciousState : NPCState
{
    protected override Color? StateHintColor => Color.yellow;

    private readonly float minDistanceToNextLocation = 4f;
    private readonly float nextLocationRadius = 15f;

    public override void EnterState(NPC npc)
    {
        npc.Movement.SetSpeed(npc.Movement.defaultSpeed + 0.8f);

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
        // If player spotted at any time -> immediate alerted
        if (npc.HasPlayerInsight())
        {
            npc.ChangeState(new AlertedState());
            return;
        }

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