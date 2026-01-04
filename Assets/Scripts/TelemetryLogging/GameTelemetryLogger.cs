using System;
using System.Collections.Concurrent;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Newtonsoft.Json;

public static class GameTelemetryLogger
{
    private static readonly ConcurrentQueue<string> logQueue = new();
    private static string filePath;
    private static bool isRunning;

    public static void Initialize()
    {
        SessionManager.Initialize();

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
            event_data = eventData
        };

        string json = JsonConvert.SerializeObject(logEvent);
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
