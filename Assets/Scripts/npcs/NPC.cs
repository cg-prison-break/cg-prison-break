using Objects.Interactables;
using UnityEngine;
using UnityEngine.AI;

public abstract class NPC : MonoBehaviour, IInteractable
{
    public Animator animator;
    public NavMeshAgent navMeshAgent;
    public float speed = 2.0f;
    public float sightRange = 5f;
    public float fieldOfViewAngle = 100f;

    protected NPCState currentState;
    protected NPCState previousState;

    public abstract string InteractionPrompt { get; set; }

    private GameObject _playerRef;

    [HideInInspector]
    public GameObject playerRef { get => _playerRef; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        NPCEventManager.OnPauseEvent += HandlePauseEvent;
        NPCEventManager.OnResumeEvent += HandleResumeEvent;

        animator = GetComponentInChildren<Animator>();
        _playerRef = GameObject.FindGameObjectWithTag("Player");

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

    public abstract void Interact(Player interactor);

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

    public bool HasPlayerInsight()
    {
        Vector3 toPlayer = playerRef.transform.position - transform.position;
        float angle = Vector3.Angle(transform.forward, toPlayer);

        // check if the player is in the field of view
        if (angle < fieldOfViewAngle / 2f)
        {
            if (toPlayer.magnitude < sightRange)
            {
                if (Physics.Raycast(transform.position, toPlayer.normalized, out RaycastHit hit, sightRange, LayerMask.GetMask("Default")))
                {
                    if (hit.collider != null && hit.collider.CompareTag("Player"))
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }
}
