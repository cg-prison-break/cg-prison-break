using System;
using UnityEngine;

public class NPCEventManager : MonoBehaviour
{
    public static event Action OnPauseEvent;
    public static event Action OnResumeEvent;
    public static event Action<Vector3> OnSuspiciousActionEvent;

    public static void PauseNPCs()
    {
        OnPauseEvent?.Invoke();
    }

    public static void ResumeNPCs()
    {
        OnResumeEvent?.Invoke();
    }

    public static void NotifyNPCsAboutSuspiciousAction(Vector3 location)
    {
        OnSuspiciousActionEvent?.Invoke(location);
    }
}