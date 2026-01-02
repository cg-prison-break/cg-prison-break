using Objects.Interactables;
using UnityEngine;

[RequireComponent(typeof(NPCMovement))]
public abstract class NPC : MonoBehaviour, IInteractable
{
    public NPCMovement Movement { get; private set; }
    [SerializeField] private float sightRange = 5f;
    [SerializeField] private float fieldOfViewAngle = 110f;

    protected NPCState currentState;
    protected NPCState previousState;

    public abstract string InteractionPrompt { get; set; }

    protected virtual void Awake()
    {
        Movement = GetComponent<NPCMovement>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        ChangeState(new IdleState());
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        currentState.UpdateState(this);
    }

    protected virtual void OnDestroy() { }

    public abstract void Interact(Player interactor);

    public void ChangeState(NPCState newState)
    {
        currentState?.ExitState(this);
        previousState = currentState;
        currentState = newState;
        currentState.EnterState(this);
    }

    public bool HasPlayerInsight()
    {
        Vector3 toPlayer = PlayerRegistry.Player.transform.position - transform.position;
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
