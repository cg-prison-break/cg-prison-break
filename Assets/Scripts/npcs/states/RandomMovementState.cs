using UnityEngine;
using UnityEngine.AI;

public class RandomMovementState : NPCState
{
    private readonly float distanceThreshold = 0.2f;
    private Vector3 currentTarget;

    protected override Color? StateHintColor => Color.green;

    public override void EnterState(NPC npc)
    {
        npc.navMeshAgent.isStopped = true;

        if (npc.navMeshAgent.hasPath)
        {
            currentTarget = npc.navMeshAgent.destination;
        }
        else
        {
            npc.navMeshAgent.ResetPath();

            currentTarget = FindRandomTargetPoint(npc);
            npc.navMeshAgent.SetDestination(currentTarget);
        }

        npc.navMeshAgent.isStopped = false;
        npc.animator.SetBool("isWalking", true);
        UpdateStateHint(npc);
    }

    public override void ExitState(NPC npc)
    {
        npc.navMeshAgent.isStopped = true;
        npc.animator.SetBool("isWalking", false);
    }

    public override void UpdateState(NPC npc)
    {
        float distance = npc.navMeshAgent.remainingDistance;
        bool destinationReached = distance <= distanceThreshold;

        if (destinationReached && !npc.navMeshAgent.pathPending)
        {
            currentTarget = FindRandomTargetPoint(npc);
            npc.navMeshAgent.SetDestination(currentTarget);
        }
    }

    private Vector3 FindRandomTargetPoint(NPC npc)
    {
        Vector3 randomDirection = npc.transform.position + Random.insideUnitSphere * 10f;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, 1, NavMesh.AllAreas);
        return hit.position;
    }
}

