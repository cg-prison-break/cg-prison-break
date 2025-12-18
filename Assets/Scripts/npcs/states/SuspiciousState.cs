using System.Collections;
using UnityEngine;

public class SuspiciousState : NPCState
{
    private readonly float distanceThreshold = 0.2f;
    private readonly Vector3 suspiciousLocation;
    private readonly float cooldownDuration;
    private readonly float searchRadius;

    private Coroutine searchCoroutine = null;

    protected override Color? StateHintColor => Color.yellow;

    public SuspiciousState(Vector3 suspiciousLocation, float cooldownDuration = 8f, float searchRadius = 2f)
    {
        this.suspiciousLocation = suspiciousLocation;
        this.cooldownDuration = cooldownDuration;
        this.searchRadius = searchRadius;
    }

    public override void EnterState(NPC npc)
    {
        if (npc.playerRef == null)
        {
            // no player found -> fallback
            npc.ChangeState(new RandomMovementState());
            return;
        }

        npc.navMeshAgent.isStopped = true;
        npc.navMeshAgent.ResetPath();
        npc.navMeshAgent.SetDestination(suspiciousLocation);

        npc.navMeshAgent.speed = npc.speed + 1.5f;
        npc.navMeshAgent.isStopped = false;
        npc.animator.SetBool("isWalking", true);

        // Reset search state when entering
        if (searchCoroutine != null)
        {
            npc.StopCoroutine(searchCoroutine);
            searchCoroutine = null;
        }
        UpdateStateHint(npc);
    }

    public override void ExitState(NPC npc)
    {
        // Stop any running search coroutine started on the NPC
        if (searchCoroutine != null)
        {
            npc.StopCoroutine(searchCoroutine);
            searchCoroutine = null;
        }

        npc.navMeshAgent.isStopped = true;
        npc.animator.SetBool("isWalking", false);
        // Ensure agent rotation behavior is normal when exiting
        npc.navMeshAgent.updateRotation = true;
        npc.navMeshAgent.speed = npc.speed;
    }

    public override void UpdateState(NPC npc)
    {
        // If player spotted at any time -> immediate alerted
        if (npc.HasPlayerInsight())
        {
            if (searchCoroutine != null)
            {
                npc.StopCoroutine(searchCoroutine);
                searchCoroutine = null;
            }
            npc.ChangeState(new AlertedState());
            return;
        }

        float distance = npc.navMeshAgent.remainingDistance;
        bool destinationReached = distance <= distanceThreshold;

        // Start the search behaviour once we reached the suspicious location
        if (destinationReached && searchCoroutine == null)
        {
            searchCoroutine = npc.StartCoroutine(SearchAndLookRoutine(npc));
        }
    }

    private IEnumerator SearchAndLookRoutine(NPC npc)
    {
        float endTime = Time.time + cooldownDuration;

        while (Time.time < endTime)
        {
            // pick a random reachable point near the suspicious location
            if (!NavMeshUtils.TryFindValidNavMeshPosition(suspiciousLocation, searchRadius, out var samplePoint))
            {
                // couldn't find navmesh sample, try next frame
                yield return null;
                continue;
            }

            // walk to the sampled point
            npc.navMeshAgent.isStopped = false;
            npc.navMeshAgent.SetDestination(samplePoint);
            npc.animator.SetBool("isWalking", true);

            // wait until agent reaches the point (or player is seen)
            while (npc.navMeshAgent.pathPending)
            {
                if (npc.HasPlayerInsight()) goto OnAlert;
                yield return null;
            }
            while (npc.navMeshAgent.hasPath && npc.navMeshAgent.remainingDistance > distanceThreshold)
            {
                if (npc.HasPlayerInsight()) goto OnAlert;
                yield return null;
            }

            // arrived: stop and do focused look (left/right)
            npc.navMeshAgent.isStopped = true;
            npc.animator.SetBool("isWalking", false);

            // perform left/right head/body turns
            yield return npc.StartCoroutine(LookLeftRight(npc));

            // small pause before next point to make movement less robotic
            yield return new WaitForSeconds(Random.Range(0.2f, 0.6f));
        }

        // cooldown finished -> resume random movement
        searchCoroutine = null;
        npc.ChangeState(new RandomMovementState());
        yield break;

    OnAlert:
        // cleanup and change to alerted
        searchCoroutine = null;
        npc.navMeshAgent.isStopped = true;
        npc.animator.SetBool("isWalking", false);
        npc.ChangeState(new AlertedState());
        yield break;
    }

    private IEnumerator LookLeftRight(NPC npc)
    {
        // Temporarily disable NavMeshAgent rotation so we can control facing
        bool prevUpdateRotation = npc.navMeshAgent.updateRotation;
        npc.navMeshAgent.updateRotation = false;

        Quaternion original = npc.transform.rotation;
        float lookAngle = 60f; // degrees left/right
        float smallTurnTime = 0.35f;
        float acrossTime = smallTurnTime * 2f;
        float pause = 0.15f;

        Quaternion left = Quaternion.Euler(0f, original.eulerAngles.y - lookAngle, 0f);
        Quaternion right = Quaternion.Euler(0f, original.eulerAngles.y + lookAngle, 0f);

        // rotate to left
        float t = 0f;
        while (t < smallTurnTime)
        {
            npc.transform.rotation = Quaternion.Slerp(original, left, t / smallTurnTime);
            t += Time.deltaTime;
            yield return null;
        }
        npc.transform.rotation = left;
        yield return new WaitForSeconds(pause);

        // sweep from left to right
        t = 0f;
        while (t < acrossTime)
        {
            npc.transform.rotation = Quaternion.Slerp(left, right, t / acrossTime);
            t += Time.deltaTime;
            yield return null;
        }
        npc.transform.rotation = right;
        yield return new WaitForSeconds(pause);

        // return to original
        t = 0f;
        while (t < smallTurnTime)
        {
            npc.transform.rotation = Quaternion.Slerp(right, original, t / smallTurnTime);
            t += Time.deltaTime;
            yield return null;
        }
        npc.transform.rotation = original;

        // restore agent rotation behaviour
        npc.navMeshAgent.updateRotation = prevUpdateRotation;
    }
}
