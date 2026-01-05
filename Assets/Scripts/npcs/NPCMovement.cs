using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class NPCMovement : MonoBehaviour
{
    public float defaultSpeed = 2f;
    private Animator animator;
    private NavMeshAgent agent;

    private Vector3 lastPosition;
    private float stuckTimer = 0f;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        SetSpeed(defaultSpeed);

        lastPosition = agent.transform.position;
    }

    public void StartWalking()
    {
        agent.isStopped = false;
        animator.SetBool("isWalkingStopped", false);
    }

    public void StopWalking()
    {
        agent.isStopped = true;
        animator.SetBool("isWalkingStopped", true);
    }

    public void SetSpeed(float speed) => agent.speed = speed;

    public bool TryMoveToDestination(Vector3 destination)
    {
         // check if destination can be reached
        NavMeshPath path = new NavMeshPath();
        bool hasPath = NavMesh.CalculatePath(
            agent.transform.position,
            destination,
            agent.areaMask,
            path
        );

        if (hasPath && path.status == NavMeshPathStatus.PathComplete)
        {
            return agent.SetDestination(destination);
        }
        else
        {
            return false;
        }
    }

    public bool PathIsValid()
    {
        return !agent.pathPending && agent.hasPath && agent.pathStatus == NavMeshPathStatus.PathComplete;
    }

    public bool HasReachedDestination(float tolerance = 0.2f)
    {
        if(Vector3.Distance(agent.transform.position, agent.destination) < tolerance)
        {
            return true;
        }

        if (agent.pathPending || !agent.hasPath)
        {
            return false;
        }

        return !agent.pathPending && agent.remainingDistance <= tolerance;
    }

    public bool IsStuck(float maxTime = 5f)
    {
        if (agent.pathPending || !agent.hasPath)
        {
            stuckTimer = 0f;
            return false;
        }

        // check if the agent has moved significantly since the last check
        float distanceTravelled = Vector3.Distance(agent.transform.position, lastPosition);

        if (distanceTravelled < 0.05f)
        {
            stuckTimer += Time.deltaTime;
        }
        else
        {
            stuckTimer = 0f;
        }

        if (stuckTimer >= maxTime)
        {
            stuckTimer = 0f;
            return true;
        }

        lastPosition = agent.transform.position;
        return false;
    }
}