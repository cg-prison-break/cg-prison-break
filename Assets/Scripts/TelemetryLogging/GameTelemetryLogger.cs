using System;
using System.Collections.Concurrent;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class GameTelemetryLogger
{
    private static readonly ConcurrentQueue<string> logQueue = new();
    private static string filePath;
    private static bool isRunning;
    private static GameData gameData;

    public static void Initialize(GameData game_data)
    {
        SessionManager.Initialize();
        gameData = game_data;

        string fileName =
            $"participant_{SessionManager.ParticipantId}_" +
            $"session_{DateTime.UtcNow:yyyyMMdd_HHmmss}.jsonl";

        filePath = Path.Combine(".", fileName);

        isRunning = true;
        StartWriterTask();

        LogTelemetryEvent(new SessionStartData(
            SessionManager.SessionId, SessionManager.ParticipantId, Application.unityVersion, Application.platform.ToString(), Application.version
        ));
    }

    public static void Shutdown()
    {
        LogTelemetryEvent(new SessionEndData());
        isRunning = false;
    }

    public static void LogTelemetryEvent(TelemetryEventDataBase eventData)
    {
        var logEvent = new TelemetryEvent
        {
            timestamp = DateTime.UtcNow.ToString("o"),
            game_time = gameData.timer,
            player_position = PlayerRegistry.Player != null ? PlayerRegistry.Player.transform.position : Vector3.zero,
            event_data = eventData
        };

        string json = JsonUtility.ToJson(logEvent);
        logQueue.Enqueue(json);
    }

    private static async void StartWriterTask()
    {
        await Task.Run(async () =>
        {
            while (isRunning || !logQueue.IsEmpty)
            {
                if (logQueue.TryDequeue(out string logLine))
                {
                    await File.AppendAllTextAsync(
                        filePath,
                        logLine + Environment.NewLine,
                        Encoding.UTF8
                    );
                }
                else
                {
                    await Task.Delay(50);
                }
            }
        });
    }
}
