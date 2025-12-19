using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class RandomMovementState : NPCState
{
    private Vector3 currentTarget;
    private bool isChoosingNextTarget = false;

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
            isChoosingNextTarget = true;
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
        if (isChoosingNextTarget || npc.navMeshAgent.pathPending)
        {
            npc.navMeshAgent.isStopped = true;
            npc.animator.SetBool("isWalking", false);
            return;
        }

        bool isMoving =
        npc.navMeshAgent.hasPath &&
        npc.navMeshAgent.pathStatus == NavMeshPathStatus.PathComplete &&
        npc.navMeshAgent.remainingDistance > 0.3f;

        if (isMoving)
        {
            npc.navMeshAgent.isStopped = false;
            npc.animator.SetBool("isWalking", true);
            return;
        }

        // only move when player is nearby
        float playerDist = Vector3.Distance(npc.transform.position, npc.playerRef.transform.position);
        if (playerDist >= 50.0f)
        {
            npc.navMeshAgent.isStopped = true;
            npc.animator.SetBool("isWalking", false);
            return;
        }

        if (!isChoosingNextTarget)
        {
            isChoosingNextTarget = true;
            npc.navMeshAgent.isStopped = false;
            npc.animator.SetBool("isWalking", true);
            npc.StartCoroutine(SetNextTarget(npc));
        }
    }

    private IEnumerator SetNextTarget(NPC npc)
    {
        yield return new WaitForSeconds(Random.Range(0f, 0.1f));
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

        isChoosingNextTarget = false;
    }
}
