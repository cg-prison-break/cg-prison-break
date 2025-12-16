using Objects.Interactables;
using UnityEngine;
using UnityEngine.AI;

public abstract class NPC : MonoBehaviour, IInteractable
{
    public Animator animator;
    public NavMeshAgent navMeshAgent;
    public float speed = 2.0f;

    protected NPCState currentState;
    protected NPCState previousState;

    public abstract string InteractionPrompt { get; set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        NPCEventManager.OnPauseEvent += HandlePauseEvent;
        NPCEventManager.OnResumeEvent += HandleResumeEvent;

        animator = GetComponentInChildren<Animator>();

        ChangeState(new IdleState());
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        currentState.UpdateState(this);
    }

    protected virtual void OnDestroy()
    {
        NPCEventManager.OnPauseEvent -= HandlePauseEvent;
        NPCEventManager.OnResumeEvent -= HandleResumeEvent;
    }

    private void HandlePauseEvent()
    {
        if (currentState is not PauseState)
        {
            ChangeState(new PauseState());
        }
    }

    private void HandleResumeEvent()
    {
        if (previousState is not null)
        {
            ChangeState(previousState);
        }
    }

    public void ChangeState(NPCState newState)
    {
        currentState?.ExitState(this);
        previousState = currentState;
        currentState = newState;
        currentState.EnterState(this);
    }

    public abstract void Interact(Player interactor);
}
