using System;
using UnityEngine;

public class NPCEventManager : MonoBehaviour
{
    public static event Action<Vector3, bool> OnSuspiciousActionEvent;
    public static event Action OnResetPrisonGuardsToSpawnStateEvent;
    public static event Action OnMakeAllPrisonGuardsSuspiciousEvent;

    public static void NotifyNPCsAboutSuspiciousAction(Vector3 location, bool global=false)
    {
        NavMeshUtils.TryFindValidNavMeshPosition(location, 2.0f, 0.1f, out var suspiciousLocation);
        OnSuspiciousActionEvent?.Invoke(suspiciousLocation, global);
    }

    public static void ResetPrisonGuardsToSpawnState()
    {
        OnResetPrisonGuardsToSpawnStateEvent?.Invoke();
    }

    public static void MakeAllPrisonGuardsAlwaysSuspcious()
    {
        OnMakeAllPrisonGuardsSuspiciousEvent?.Invoke();
    }
}