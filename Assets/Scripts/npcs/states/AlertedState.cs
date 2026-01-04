using UnityEngine;

public class AlertedState : NPCState
{
    private readonly float catchDistance = 1.2f;
    private readonly float catchDuration = 2.0f;
    private readonly float lostSightDuration = 3.0f;

    private float catchTimer = 0f;
    private float lostSightTimer = 0f;
    private bool isCatching = false;

    private Vector3 lastKnownPlayerPosition = Vector3.zero;

    protected override Color? StateHintColor => Color.red;

    public override void EnterState(NPC npc)
    {
        if (npc.playerRef == null)
        {
            // no player found -> fallback
            npc.ChangeState(new RandomMovementState());
            return;
        }

        npc.navMeshAgent.speed = npc.speed + 2f;
        npc.navMeshAgent.stoppingDistance = catchDistance;
        npc.navMeshAgent.isStopped = false;

        npc.animator.SetBool("isWalking", true);

        isCatching = false;
        catchTimer = 0f;
        lostSightTimer = 0f;

        UpdateStateHint(npc);

        GameTelemetryLogger.LogTelemetryEvent(new AlertedPrisonGuardData(npc.transform.position));
    }

    public override void ExitState(NPC npc)
    {
        npc.navMeshAgent.isStopped = false;
        npc.navMeshAgent.speed = npc.speed;

        npc.animator.SetBool("isWalking", false);

        isCatching = false;
        catchTimer = 0f;
        lostSightTimer = 0f;
    }

    public override void UpdateState(NPC npc)
    {
        // If already in catching sequence, count down then switch state
        if (isCatching)
        {
            catchTimer += Time.deltaTime;
            if (catchTimer >= catchDuration)
            {
                // after catching, resume normal behaviour
                npc.ChangeState(new RandomMovementState());
            }
            return;
        }

        if (npc.HasPlayerInsight())
        {
            lastKnownPlayerPosition = npc.playerRef.transform.position;
            lostSightTimer = 0f;

            // set destination to player's current position (chase)
            npc.navMeshAgent.isStopped = false;
            npc.navMeshAgent.SetDestination(npc.playerRef.transform.position);
            npc.animator.SetBool("isWalking", true);
        }
        else
        {
            lostSightTimer += Time.deltaTime;

            if (lostSightTimer < lostSightDuration)
            {
                npc.navMeshAgent.isStopped = false;
                npc.navMeshAgent.SetDestination(lastKnownPlayerPosition);
                npc.animator.SetBool("isWalking", true);
            }
            else
            {
                // player lost for too long, switch to suspicious state
                npc.ChangeState(new SuspiciousState(lastKnownPlayerPosition));
                return;
            }
        }

        // check distance
        float dist = Vector3.Distance(npc.transform.position, npc.playerRef.transform.position);
        if (dist <= catchDistance)
        {
            // start catching
            isCatching = true;
            catchTimer = 0f;

            // stop moving and play catch/idle animation
            npc.navMeshAgent.isStopped = true;
            npc.animator.SetBool("isWalking", false);

            // notify player object that it was caught
            npc.playerRef.SendMessage("OnCaught", SendMessageOptions.DontRequireReceiver);
        }
    }
}
