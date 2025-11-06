using System;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using UnityEngine.AI;
using Action = Unity.Behavior.Action;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "RandomPatrol", story: "[Agent] patrols randomly", category: "Action/Navigation", id: "c220b784bea1c6a5e97a6612819dc81d")]
public partial class RandomPatrolAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<float> Speed = new(3f);
    [SerializeReference] public BlackboardVariable<float> DistanceThreshold = new(0.2f);

    private NavMeshAgent navMeshAgent;
    private Vector3 currentTarget;

    protected override Status OnStart()
    {
        navMeshAgent = Agent.Value.GetComponentInChildren<NavMeshAgent>();
        if (navMeshAgent != null)
        {
            if (navMeshAgent.isOnNavMesh)
            {
                navMeshAgent.ResetPath();
            }

            navMeshAgent.speed = Speed.Value;
            navMeshAgent.stoppingDistance = DistanceThreshold;

            currentTarget = FindRandomTargetPoint();
            navMeshAgent.SetDestination(currentTarget);
        }

        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        float distance = navMeshAgent.remainingDistance;
        bool destinationReached = distance <= DistanceThreshold;

        // Check if we've reached the waypoint (ensuring NavMeshAgent has completed path calculation if available)
        if (destinationReached && (navMeshAgent == null || !navMeshAgent.pathPending))
        {
            currentTarget = FindRandomTargetPoint();
            navMeshAgent.SetDestination(currentTarget);
        }
        return Status.Running;
    }

    protected override void OnEnd()
    {
    }

    private Vector3 FindRandomTargetPoint()
    {
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * 20f;
        randomDirection += Agent.Value.transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, 20f, NavMesh.AllAreas);
        return hit.position;
    }
}

