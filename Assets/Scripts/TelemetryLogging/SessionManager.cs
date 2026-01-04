using System;
using UnityEngine;

public static class SessionManager
{
    public static string SessionId { get; private set; }
    public static string ParticipantId { get; private set; }

    public static void Initialize()
    {
        SessionId = Guid.NewGuid().ToString();

        ParticipantId = PlayerPrefs.GetString(
            "participant_id",
            "P_" + UnityEngine.Random.Range(100000, 999999)
        );

        PlayerPrefs.SetString("participant_id", ParticipantId);
    }
}
