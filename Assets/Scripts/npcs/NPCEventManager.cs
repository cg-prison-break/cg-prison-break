using System;
using UnityEngine;

public class NPCEventManager : MonoBehaviour
{
    public static event Action<Vector3, bool> OnSuspiciousActionEvent;
    public static event Action OnResetSuspiciousPrisonGuardsEvent;
    public static event Action OnAlertAllPrisonGuardsEvent;

    public static void NotifyNPCsAboutSuspiciousAction(Vector3 location, bool global=false)
    {
        OnSuspiciousActionEvent?.Invoke(location, global);
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