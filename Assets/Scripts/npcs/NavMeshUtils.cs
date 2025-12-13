using UnityEngine;
using UnityEngine.AI;

public class NavMeshUtils
{
    public static bool TryFindValidNavMeshPosition(Vector3 center, float radius, out Vector3 result)
    {
        const int attempts = 10;
        result = Vector3.zero;

        for (int i = 0; i < attempts; i++)
        {
            Vector3 randomOffset = Random.insideUnitSphere * radius;
            randomOffset.y = 0f;
            Vector3 samplePoint = center + randomOffset;

            if (NavMesh.SamplePosition(samplePoint, out NavMeshHit hit, radius, NavMesh.AllAreas))
            {
                if (IsFinite(hit.position))
                {
                    result = hit.position;
                    return true;
                }
            }
        }

        return false;
    }

    private static bool IsFinite(Vector3 v)
    {
        return !(float.IsNaN(v.x) || float.IsNaN(v.y) || float.IsNaN(v.z)
                 || float.IsInfinity(v.x) || float.IsInfinity(v.y) || float.IsInfinity(v.z));
    }
}
