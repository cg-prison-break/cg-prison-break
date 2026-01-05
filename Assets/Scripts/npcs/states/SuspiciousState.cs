using System.Collections;
using UnityEngine;

public class SuspiciousState : NPCState
{
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
        npc.Movement.SetSpeed(npc.Movement.defaultSpeed + 1.5f);
        npc.Movement.TryMoveToDestination(suspiciousLocation);
        npc.Movement.StartWalking();

        UpdateStateHint(npc);

        GameTelemetryLogger.LogTelemetryEvent(new SuspiciousPrisonGuardData(npc.transform.position));
    }

    public override void ExitState(NPC npc)
    {
        // Stop any running search coroutine started on the NPC
        if (searchCoroutine != null)
        {
            npc.StopCoroutine(searchCoroutine);
            searchCoroutine = null;
        }

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

        // If stuck while going to suspicious location -> abandon and go to random movement
        if (npc.Movement.IsStuck())
        {
            npc.ChangeState(npc.SpawnState);
            return;
        }

        // Start the search behaviour once we reached the suspicious location
        if (npc.Movement.HasReachedDestination() && searchCoroutine == null)
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
            if (!NavMeshUtils.TryFindValidNavMeshPosition(suspiciousLocation, searchRadius, 0.2f, out var samplePoint))
            {
                // couldn't find navmesh sample, try next frame
                yield return null;
                continue;
            }

            // walk to the sampled point
            npc.Movement.TryMoveToDestination(samplePoint);

            // wait until agent reaches the point (or player is seen)
            while (!npc.Movement.HasReachedDestination())
            {
                if (npc.HasPlayerInsight())
                {
                    // change to alerted
                    searchCoroutine = null;
                    npc.ChangeState(new AlertedState());
                    yield break;
                }

                if (npc.Movement.IsStuck())
                {
                    // stuck -> abandon search
                    searchCoroutine = null;
                    npc.ChangeState(npc.SpawnState);
                    yield break;
                }

                yield return null;
            }

            // arrived: stop and do focused look (left/right)
            npc.Movement.StopWalking();

            // perform left/right head/body turns
            yield return npc.StartCoroutine(LookLeftRight(npc));

            // small pause before next point to make movement less robotic
            yield return new WaitForSeconds(Random.Range(0.2f, 0.6f));
        }

        // cooldown finished -> resume random movement
        searchCoroutine = null;
        npc.ChangeState(npc.SpawnState);
        yield break;
    }

    private IEnumerator LookLeftRight(NPC npc)
    {
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
            if (npc.HasPlayerInsight())
            {
                // restore rotation behaviour and alert
                searchCoroutine = null;
                npc.transform.rotation = original;
                npc.ChangeState(new AlertedState());
                yield break;
            }
            npc.transform.rotation = Quaternion.Slerp(original, left, t / smallTurnTime);
            t += Time.deltaTime;
            yield return null;
        }
        npc.transform.rotation = left;

        // pause but check sight each frame
        float pp = 0f;
        while (pp < pause)
        {
            if (npc.HasPlayerInsight())
            {
                searchCoroutine = null;
                npc.transform.rotation = original;
                npc.ChangeState(new AlertedState());
                yield break;
            }
            pp += Time.deltaTime;
            yield return null;
        }

        // sweep from left to right
        t = 0f;
        while (t < acrossTime)
        {
            if (npc.HasPlayerInsight())
            {
                // restore rotation behaviour and alert
                searchCoroutine = null;
                npc.transform.rotation = original;
                npc.ChangeState(new AlertedState());
                yield break;
            }
            npc.transform.rotation = Quaternion.Slerp(left, right, t / acrossTime);
            t += Time.deltaTime;
            yield return null;
        }
        npc.transform.rotation = right;

        // pause but check sight each frame
        pp = 0f;
        while (pp < pause)
        {
            if (npc.HasPlayerInsight())
            {
                searchCoroutine = null;
                npc.transform.rotation = original;
                npc.ChangeState(new AlertedState());
                yield break;
            }
            pp += Time.deltaTime;
            yield return null;
        }

        // return to original
        t = 0f;
        while (t < smallTurnTime)
        {
            if (npc.HasPlayerInsight())
            {
                // restore rotation behaviour and alert
                searchCoroutine = null;
                npc.transform.rotation = original;
                npc.ChangeState(new AlertedState());
                yield break;
            }
            npc.transform.rotation = Quaternion.Slerp(right, original, t / smallTurnTime);
            t += Time.deltaTime;
            yield return null;
        }
        npc.transform.rotation = original;
    }
}
