using System;
using UnityEngine;

public enum TelemetryEventType
{
    SessionStart,
    SessionEnd,
    GameWon,
    GameOver,
    PlayerCaught,
    NPCInteracted,
    ItemCollected,
    ItemDropped,
    ItemUsed,
    SpreadsheetOpened,
    SpreadsheetRouteClicked,
    RoomEntered,
    SuspiciousEventTriggered,
    SuspiciousPrisonGuard,
    AlertedPrisonGuard
}

[Serializable]
public class TelemetryEventDataBase
{
    public string eventType;
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
        eventType = TelemetryEventType.SessionStart.ToString();
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
        eventType = TelemetryEventType.SessionEnd.ToString();
    }
}

[Serializable]
public class GameWonData : TelemetryEventDataBase
{
    public int strikes { get; }

    public GameWonData(int strikes)
    {
        eventType = TelemetryEventType.GameWon.ToString();
        this.strikes = strikes;
    }
}

[Serializable]
public class GameOverData : TelemetryEventDataBase
{
    public GameOverData()
    {
        eventType = TelemetryEventType.GameOver.ToString();
    }
}

[Serializable]
public class PlayerCaughtData : TelemetryEventDataBase
{
    public int strike { get; }

    public PlayerCaughtData(int strike)
    {
        eventType = TelemetryEventType.PlayerCaught.ToString();
        this.strike = strike;
    }
}

[Serializable]
public class NPCInteractedData : TelemetryEventDataBase
{
    public string npc_type { get; }

    public NPCInteractedData(NPC npc)
    {
        eventType = TelemetryEventType.NPCInteracted.ToString();

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
public class ItemCollectedData : TelemetryEventDataBase
{
    public string item_name { get; }

    public ItemCollectedData(string itemName)
    {
        eventType = TelemetryEventType.ItemCollected.ToString();
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
        eventType = TelemetryEventType.ItemDropped.ToString();
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
        eventType = TelemetryEventType.ItemUsed.ToString();
        item_name = itemName;
    }
}

[Serializable]
public class SpreadsheetOpenedData : TelemetryEventDataBase
{
    public SpreadsheetOpenedData()
    {
        eventType = TelemetryEventType.SpreadsheetOpened.ToString();
    }
}

[Serializable]
public class SpreadsheetRouteClickedData : TelemetryEventDataBase
{
    public string route_name { get; }

    public SpreadsheetRouteClickedData(string routeName)
    {
        eventType = TelemetryEventType.SpreadsheetRouteClicked.ToString();
        route_name = routeName;
    }
}

[Serializable]
public class RoomEnteredData : TelemetryEventDataBase
{
    public string room_name { get; }

    public RoomEnteredData(string roomName)
    {
        eventType = TelemetryEventType.RoomEntered.ToString();
        room_name = roomName;
    }
}

[Serializable]
public class SuspiciousEventTriggeredData : TelemetryEventDataBase
{
    public string reason { get; }
    public SuspiciousEventTriggeredData(string eventName)
    {
        eventType = TelemetryEventType.SuspiciousEventTriggered.ToString();
        reason = eventName;
    }
}

[Serializable]
public class SuspiciousPrisonGuardData : TelemetryEventDataBase
{
    public Vector3 prison_guard_location { get; }

    public SuspiciousPrisonGuardData(Vector3 location)
    {
        eventType = TelemetryEventType.SuspiciousPrisonGuard.ToString();
        prison_guard_location = location;
    }
}

[Serializable]
public class AlertedPrisonGuardData : TelemetryEventDataBase
{
    public Vector3 prison_guard_location { get; }

    public AlertedPrisonGuardData(Vector3 location)
    {
        eventType = TelemetryEventType.AlertedPrisonGuard.ToString();
        prison_guard_location = location;
    }
}