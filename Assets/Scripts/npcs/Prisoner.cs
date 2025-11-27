using UnityEngine;

public class Prisoner : NPC
{
    protected override void Start()
    {
        base.Start();
        ChangeState(new RandomMovementState());
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }

    public override string InteractionPrompt { get; set; }

    public override void Interact(Player interactor)
    {

    }
}
