using System;

public static class SessionManager
{
    public static string SessionId { get; private set; }
    public static string ParticipantId { get; private set; }

    public static void Initialize()
    {
        SessionId = Guid.NewGuid().ToString();
        ParticipantId = "P_" + UnityEngine.Random.Range(100000, 999999);
    }
}
