using Sounds.NPCs;
using UnityEngine;

public class Prisoner : NPC
{
    [Header("Sounds")]
    public AudioSource audioSource;
    public NPCInteractionSoundSet soundSet;

    private AudioClip interactionAudioClip;
    private readonly string[] variants = new string[] { "prisoner", "prisoner1", "prisoner5", "prisoner6" };

    protected override void Awake()
    {
        base.Awake();

        var rnd = new System.Random();
        interactionAudioClip = soundSet.interactionSounds[rnd.Next(soundSet.interactionSounds.Length)];

        // select a random variant
        var variant = variants[rnd.Next(variants.Length)];

        foreach (Transform child in transform.GetChild(0))
        {
            bool shouldBeActive = child.name == variant;
            child.gameObject.SetActive(shouldBeActive);
        }
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }

    public override string InteractionPrompt { 
        get => "Drücke F zum Interagieren."; 
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
