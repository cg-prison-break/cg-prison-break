using UnityEngine;

public class Prisoner : NPC
{
    private readonly string[] variants = new string[] { "prisoner", "prisoner1", "prisoner5", "prisoner6" };

    void Awake()
    {
        // select a random variant
        var rnd = new System.Random();
        var variant = variants[rnd.Next(variants.Length)];

        foreach (Transform child in transform)
        {
            bool shouldBeActive = child.name == variant;
            child.gameObject.SetActive(shouldBeActive);
        }
    }

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
