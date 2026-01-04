using UnityEngine;

public class AlertedState : NPCState
{
    private readonly float catchDistance = 1.2f;
    private readonly float catchDuration = 2.0f;
    private readonly float lostSightDuration = 4.0f;

    private float catchTimer = 0f;
    private float lostSightTimer = 0f;

    private Vector3 lastKnownPlayerPosition = Vector3.zero;

    protected override Color? StateHintColor => Color.red;

    public override void EnterState(NPC npc)
    {
        npc.Movement.SetSpeed(npc.Movement.defaultSpeed + 3f);
        npc.Movement.StartWalking();

        catchTimer = 0f;
        lostSightTimer = 0f;

        UpdateStateHint(npc);

        GameTelemetryLogger.LogTelemetryEvent(new AlertedPrisonGuardData(npc.transform.position));
    }

    public override void ExitState(NPC npc)
    {
        npc.Movement.StopWalking();
        npc.Movement.SetSpeed(npc.Movement.defaultSpeed);

        catchTimer = 0f;
        lostSightTimer = 0f;
    }

    public override void UpdateState(NPC npc)
    {
        if (npc.HasPlayerInsight())
        {
            if (NavMeshUtils.IsPositionOnNavMesh(PlayerRegistry.Player.transform.position))
            {
                lastKnownPlayerPosition = PlayerRegistry.Player.transform.position;
            }
            else
            {
                NavMeshUtils.TryFindValidNavMeshPosition(PlayerRegistry.Player.transform.position, 0.1f, 0.001f, out lastKnownPlayerPosition);
            }
            lostSightTimer = 0f;

            // set destination to player's current position (chase)
            npc.Movement.TryMoveToDestination(lastKnownPlayerPosition);
        }
        else
        {
            lostSightTimer += Time.deltaTime;

            if (lostSightTimer < lostSightDuration)
            {
                npc.Movement.TryMoveToDestination(lastKnownPlayerPosition);
            }
            else
            {
                // player lost for too long, switch to suspicious state
                npc.ChangeState(new SuspiciousState(lastKnownPlayerPosition));
                return;
            }
        }

        if (npc.Movement.IsStuck())
        {
            // if stuck, switch to suspicious state
            npc.ChangeState(new SuspiciousState(lastKnownPlayerPosition));
            return;
        }

        // check distance
        float dist = Vector3.Distance(npc.transform.position, PlayerRegistry.Player.transform.position);
        if (dist <= catchDistance)
        {
            // start catching
            catchTimer += Time.deltaTime;

            if (catchTimer >= catchDuration)
            {
                // notify player object that it was caught
                PlayerRegistry.Player.OnCaught(npc.GetComponent<AudioSource>());
                npc.ChangeState(new IdleState());
                return;
            }
        }
        else
        {
            catchTimer = 0f;
        }
    }
}
