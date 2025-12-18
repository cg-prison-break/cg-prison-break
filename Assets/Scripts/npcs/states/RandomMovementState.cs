using System.Collections;
using UnityEngine;

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
            npc.StartCoroutine(SetNextTarget(npc));
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
            npc.StartCoroutine(SetNextTarget(npc));
        }
    }

    private IEnumerator SetNextTarget(NPC npc)
    {
        yield return new WaitForSeconds(Random.Range(0f, 0.3f));
        if (NavMeshUtils.TryFindValidNavMeshPosition(npc.transform.position, 15f, out var nextTarget))
        {
            currentTarget = nextTarget;
            npc.navMeshAgent.SetDestination(currentTarget);
        }
        else
        {
            // no target found, try again in next update
            npc.navMeshAgent.ResetPath();
        }
    }
}
