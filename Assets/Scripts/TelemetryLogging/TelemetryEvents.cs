using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using UnityEngine;

public enum TelemetryEventType
{
    SessionStart,
    SessionEnd,
    GameStart,
    GameWon,
    GameOver,
    PlayerCaught,
    NPCInteracted,
    ItemPickedUp,
    ItemDropped,
    ItemUsed,
    SpreadsheetOpened,
    SpreadsheetClosed,
    SpreadsheetRouteClicked,
    SuspiciousEventTriggered,
    SuspiciousPrisonGuard,
    AlertedPrisonGuard,
    GeneratorShutdown
}

[Serializable]
public class TelemetryEventDataBase
{
    [JsonConverter(typeof(StringEnumConverter))] public TelemetryEventType eventType;
}

[Serializable]
public class TelemetryEvent
{
    public string timestamp;
    public float game_time;
    public Vector3 player_position;
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

[Serializable]
public class GameStartData : TelemetryEventDataBase
{
    public GameStartData()
    {
        eventType = TelemetryEventType.GameStart;
    }
}

[Serializable]
public class GameWonData : TelemetryEventDataBase
{
    public int strikes { get; }
    public bool secret_escape_used { get; }

    public GameWonData(int strikes, bool secret_escape_used)
    {
        eventType = TelemetryEventType.GameWon;
        this.strikes = strikes;
        this.secret_escape_used = secret_escape_used;
    }
}

[Serializable]
public class GameOverData : TelemetryEventDataBase
{
    public GameOverData()
    {
        eventType = TelemetryEventType.GameOver;
    }
}

[Serializable]
public class PlayerCaughtData : TelemetryEventDataBase
{
    public int strike { get; }

    public PlayerCaughtData(int strike)
    {
        eventType = TelemetryEventType.PlayerCaught;
        this.strike = strike;
    }
}

[Serializable]
public class NPCInteractedData : TelemetryEventDataBase
{
    public string npc_type { get; }

    public NPCInteractedData(NPC npc)
    {
        eventType = TelemetryEventType.NPCInteracted;

        if (npc is Prisoner)
        {
            npc_type = "Prisoner";
        }
        else if (npc is PrisonGuard)
        {
            npc_type = "PrisonGuard";
        }
    }
}

[Serializable]
public class ItemPickedUpData : TelemetryEventDataBase
{
    public string item_name { get; }

    public ItemPickedUpData(string itemName)
    {
        eventType = TelemetryEventType.ItemPickedUp;
        item_name = itemName;
    }
}

[Serializable]
public class ItemDroppedData : TelemetryEventDataBase
{
    public string item_name { get; }
    public int slot { get; }

    public ItemDroppedData(string itemName, int slot)
    {
        eventType = TelemetryEventType.ItemDropped;
        item_name = itemName;
        this.slot = slot;
    }
}

[Serializable]
public class ItemUsedData : TelemetryEventDataBase
{
    public string item_name { get; }

    public ItemUsedData(string itemName)
    {
        eventType = TelemetryEventType.ItemUsed;
        item_name = itemName;
    }
}

[Serializable]
public class SpreadsheetOpenedData : TelemetryEventDataBase
{
    public SpreadsheetOpenedData()
    {
        eventType = TelemetryEventType.SpreadsheetOpened;
    }
}

[Serializable]
public class SpreadsheetClosedData : TelemetryEventDataBase
{
    public SpreadsheetClosedData()
    {
        eventType = TelemetryEventType.SpreadsheetClosed;
    }
}

[Serializable]
public class SpreadsheetRouteClickedData : TelemetryEventDataBase
{
    public string route_name { get; }

    public SpreadsheetRouteClickedData(string routeName)
    {
        eventType = TelemetryEventType.SpreadsheetRouteClicked;
        route_name = routeName;
    }
}

[Serializable]
public class SuspiciousEventTriggeredData : TelemetryEventDataBase
{
    public string reason { get; }
    public SuspiciousEventTriggeredData(string reason)
    {
        eventType = TelemetryEventType.SuspiciousEventTriggered;
        this.reason = reason;
    }
}

[Serializable]
public class SuspiciousPrisonGuardData : TelemetryEventDataBase
{
    public Vector3 prison_guard_location { get; }

    public SuspiciousPrisonGuardData(Vector3 location)
    {
        eventType = TelemetryEventType.SuspiciousPrisonGuard;
        prison_guard_location = location;
    }
}

[Serializable]
public class AlertedPrisonGuardData : TelemetryEventDataBase
{
    public Vector3 prison_guard_location { get; }

    public AlertedPrisonGuardData(Vector3 location)
    {
        eventType = TelemetryEventType.AlertedPrisonGuard;
        prison_guard_location = location;
    }
}

[Serializable]
public class GeneratorShutdownData : TelemetryEventDataBase
{
    public GeneratorShutdownData()
    {
        eventType = TelemetryEventType.GeneratorShutdown;
    }
}
