using UnityEngine;
using UnityEngine.AI;

public class NavMeshUtils
{
    public static bool TryFindValidNavMeshPosition(Vector3 center, float radius, float minDistance, out Vector3 result)
    {
        const int attempts = 5;
        result = Vector3.zero;

        for (int i = 0; i < attempts; i++)
        {
            Vector3 randomOffset = Random.insideUnitSphere * radius;
            randomOffset.y = 0f;
            Vector3 samplePoint = center + randomOffset;

            if (NavMesh.SamplePosition(samplePoint, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
            {
                if (IsFinite(hit.position))
                {
                    float distance = Vector3.Distance(center, hit.position);

                    if (distance >= minDistance && distance <= radius)
                    {
                        result = hit.position;
                        return true;
                    }
                }
            }
        }

        return false;
    }

    public static bool IsPositionOnNavMesh(Vector3 position)
    {
        return NavMesh.SamplePosition(position, out var hit, 0.1f, NavMesh.AllAreas);
    }

    private static bool IsFinite(Vector3 v)
    {
        return !(float.IsNaN(v.x) || float.IsNaN(v.y) || float.IsNaN(v.z)
                 || float.IsInfinity(v.x) || float.IsInfinity(v.y) || float.IsInfinity(v.z));
    }
}
