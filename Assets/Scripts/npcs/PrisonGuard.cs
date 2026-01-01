using Sounds.NPCs;
using UnityEngine;

public class PrisonGuard : NPC
{
    [Header("Sounds")]
    public AudioSource audioSource;
    public NPCInteractionSoundSet soundSet;

    private readonly float suspiciousRange = 15f;

    protected override void Start()
    {
        base.Start();

        NPCEventManager.OnSuspiciousActionEvent += HandleSuspiciousEvent;
        NPCEventManager.OnResetSuspiciousPrisonGuardsEvent += ResetToRandomMovement;
        NPCEventManager.OnAlertAllPrisonGuardsEvent += HandleAlertAllEvent;

        ChangeState(new RandomMovementState());
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        NPCEventManager.OnSuspiciousActionEvent -= HandleSuspiciousEvent;
        NPCEventManager.OnResetSuspiciousPrisonGuardsEvent -= ResetToRandomMovement;
        NPCEventManager.OnAlertAllPrisonGuardsEvent -= HandleAlertAllEvent;
    }

    private void HandleSuspiciousEvent(Vector3 location, bool global = false)
    {
        if (global)
        {
            ChangeState(new SuspiciousState(location));
        }
        else
        {
            // check if suspicious event location is nearby
            float distance = Vector3.Distance(transform.position, location);

            if (distance < suspiciousRange)
            {
                ChangeState(new SuspiciousState(location));
            }
        }
    }

    private void ResetToRandomMovement()
    {
        if (currentState is not RandomMovementState)
        {
            ChangeState(new RandomMovementState());
        }
    }

    private void HandleAlertAllEvent()
    {
        ChangeState(new AlertedState());
    }

    public override string InteractionPrompt
    {
        get => "Drücke F zum Interagieren.";
        set => InteractionPrompt = value;
    }

    public override void Interact(Player interactor)
    {
        // allow interaction only during random movement
        if (currentState is RandomMovementState)
        {
            ChangeState(new TalkingState(audioSource, soundSet, currentState));
        }
    }
}
