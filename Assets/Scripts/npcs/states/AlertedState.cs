using UnityEngine;

public class AlertedState : NPCState
{
    private readonly float catchDistance = 1.2f;
    private readonly float catchDuration = 2.0f;
    private float catchTimer = 0f;
    private bool isCatching = false;

    private GameObject playerRef;

    protected override Color? StateHintColor => Color.red;

    public override void EnterState(NPC npc)
    {
        // find player
        playerRef = GameObject.FindGameObjectWithTag("Player");
        if (playerRef == null)
        {
            // no player found -> fallback
            npc.ChangeState(new RandomMovementState());
            return;
        }

        npc.navMeshAgent.speed = npc.speed + 2.5f;
        npc.navMeshAgent.stoppingDistance = catchDistance;
        npc.navMeshAgent.isStopped = false;

        npc.animator.SetBool("isWalking", true);

        isCatching = false;
        catchTimer = 0f;

        UpdateStateHint(npc);
    }

    public override void ExitState(NPC npc)
    {
        npc.navMeshAgent.isStopped = false;

        npc.animator.SetBool("isWalking", false);

        isCatching = false;
        catchTimer = 0f;
    }

    public override void UpdateState(NPC npc) {
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

        // set destination to player's current position (chase)
        npc.navMeshAgent.isStopped = false;
        npc.navMeshAgent.SetDestination(playerRef.transform.position);
        npc.animator.SetBool("isWalking", true);

        // check distance
        float dist = Vector3.Distance(npc.transform.position, playerRef.transform.position);
        if (dist <= catchDistance)
        {
            // start catching
            isCatching = true;
            catchTimer = 0f;

            // stop moving and play catch/idle animation
            npc.navMeshAgent.isStopped = true;
            npc.animator.SetBool("isWalking", false);

            // notify player object that it was caught
            playerRef.SendMessage("OnCaught", SendMessageOptions.DontRequireReceiver);
        }
    }
}
