using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

public enum TelemetryEventType
{
    SessionStart,
    SessionEnd,
}

[Serializable]
public class TelemetryEventDataBase
{
    public string schema_version = "1.0";

    [JsonConverter(typeof(StringEnumConverter))]
    public TelemetryEventType eventType;
}

[Serializable]
public class TelemetryEvent
{
    public string timestamp;
    public TelemetryEventDataBase event_data;
}

[Serializable]
public class SessionStartData : TelemetryEventDataBase
{
    public string session_id { get; }
    public string participant_id { get; }
    public string unity_version { get; }
    public string platform { get; }
    public string game_version { get; }

    public SessionStartData(string sessionId, string participantId, string unityVersion, string platform, string gameVersion)
    {
        eventType = TelemetryEventType.SessionStart;
        session_id = sessionId ?? string.Empty;
        participant_id = participantId ?? string.Empty;
        unity_version = unityVersion ?? string.Empty;
        this.platform = platform ?? string.Empty;
        game_version = gameVersion ?? string.Empty;
    }
}

[Serializable]
public class SessionEndData : TelemetryEventDataBase
{
    public SessionEndData()
    {
        eventType = TelemetryEventType.SessionEnd;
    }
}