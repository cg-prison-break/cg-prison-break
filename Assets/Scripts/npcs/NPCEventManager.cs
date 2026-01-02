using System;
using UnityEngine;

public class NPCEventManager : MonoBehaviour
{
    public static event Action<Vector3, bool> OnSuspiciousActionEvent;
    public static event Action OnResetSuspiciousPrisonGuardsEvent;
    public static event Action OnAlertAllPrisonGuardsEvent;

    public static void NotifyNPCsAboutSuspiciousAction(Vector3 location, bool global=false)
    {
        NavMeshUtils.TryFindValidNavMeshPosition(location, 2.0f, 0.1f, out var suspiciousLocation);
        OnSuspiciousActionEvent?.Invoke(suspiciousLocation, global);
    }

    public static void ResetPrisonGuardSuspicioness()
    {
        OnResetSuspiciousPrisonGuardsEvent?.Invoke();
    }

    public static void AlertAllPrisonGuards()
    {
        OnAlertAllPrisonGuardsEvent?.Invoke();
    }
}