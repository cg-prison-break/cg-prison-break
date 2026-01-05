using Objects.Interactables;
using UnityEngine;

[RequireComponent(typeof(NPCMovement))]
public abstract class NPC : MonoBehaviour, IInteractable
{
    public NPCMovement Movement { get; private set; }
    [SerializeField] private float sightRange = 10f;
    [SerializeField] private float horizontalFOV = 110f;
    [SerializeField] private float verticalFOV = 60f;

    [HideInInspector]
    public NPCState SpawnState = new IdleState();
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
        if (SpawnState != null)
        {
            ChangeState(SpawnState);
        }
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

        // check if player is within sight range
        if (toPlayer.magnitude > sightRange)
        {
            return false;
        }

        // check if player is within field of view
        Vector3 flatForward = Vector3.ProjectOnPlane(transform.forward, Vector3.up);
        Vector3 flatToPlayer = Vector3.ProjectOnPlane(toPlayer.normalized, Vector3.up);

        float horizontalAngle = Vector3.Angle(flatForward, flatToPlayer);
        if (horizontalAngle > horizontalFOV * 0.5f)
        {
            return false;
        }

        float verticalAngle = Vector3.Angle(toPlayer.normalized, flatToPlayer);
        if (verticalAngle > verticalFOV * 0.5f)
        {
            return false;
        }

        // check if NPC has line of sight to player
        if (Physics.SphereCast(transform.position, 0.5f, toPlayer.normalized, out RaycastHit hit, sightRange, LayerMask.GetMask("Default"), QueryTriggerInteraction.Ignore))
        {
            if (hit.collider != null && hit.collider.CompareTag("Player"))
            {
                return true;
            }
        }
    
        return false;
    }
}
