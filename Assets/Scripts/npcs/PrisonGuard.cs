using Sounds.NPCs;
using UnityEngine;

public class PrisonGuard : NPC
{
    [SerializeField] private float suspiciousRange = 20f;

    [Header("Sounds")]
    public AudioSource audioSource;
    public NPCInteractionSoundSet soundSet;

    private AudioClip interactionAudioClip;

    protected override void Awake()
    {
        base.Awake();

        var rnd = new System.Random();
        interactionAudioClip = soundSet.interactionSounds[rnd.Next(soundSet.interactionSounds.Length)];
    }

    protected override void Start()
    {
        base.Start();

        NPCEventManager.OnSuspiciousActionEvent += HandleSuspiciousEvent;
        NPCEventManager.OnResetPrisonGuardsToSpawnStateEvent += ResetToSpawnState;
        NPCEventManager.OnMakeAllPrisonGuardsSuspiciousEvent += HandleAllAlywaysSuspicousEvent;
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        NPCEventManager.OnSuspiciousActionEvent -= HandleSuspiciousEvent;
        NPCEventManager.OnResetPrisonGuardsToSpawnStateEvent -= ResetToSpawnState;
        NPCEventManager.OnMakeAllPrisonGuardsSuspiciousEvent -= HandleAllAlywaysSuspicousEvent;
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

    private void ResetToSpawnState()
    {
        if (currentState != SpawnState)
        {
            ChangeState(SpawnState);
        }
    }

    private void HandleAllAlywaysSuspicousEvent()
    {
        ChangeState(new AlwaysSuspiciousState());
    }

    public override string InteractionPrompt
    {
        get => currentState is RandomMovementState ? "Drücke F zum Interagieren." : "";
        set => InteractionPrompt = value;
    }

    public override void Interact(Player interactor)
    {
        GameTelemetryLogger.LogTelemetryEvent(new NPCInteractedData(this));

        // allow interaction only during random movement
        if (currentState is RandomMovementState)
        {
            ChangeState(new TalkingState(audioSource, interactionAudioClip, currentState));
        }
    }
}
