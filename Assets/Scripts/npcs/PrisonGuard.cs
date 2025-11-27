using UnityEngine;

public class PrisonGuard : NPC
{
    [Range(0.0f, 1.0f)]
    public float attention = 0.5f;
    public float maxAttentionRange = 30.0f;
    public float sightRange = 3f;
    public float fieldOfViewAngle = 30f;

    protected override void Start()
    {
        base.Start();

        NPCEventManager.OnSuspiciousActionEvent += HandleSuspiciousEvent;

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
    }

    private void HandleSuspiciousEvent(Vector3 location)
    {
        // check if suspicious event location is nearby
        float distance = Vector3.Distance(transform.position, location);

        if (distance < attention * maxAttentionRange)
        {
            ChangeState(new SuspiciousState(location, sightRange, fieldOfViewAngle));
        }
    }

    public override string InteractionPrompt { get; set; }

    public override void Interact(Player interactor)
    {
        
    }
}
