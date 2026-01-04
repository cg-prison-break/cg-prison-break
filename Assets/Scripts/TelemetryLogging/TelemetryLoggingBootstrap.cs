using UnityEngine;

public class TelemetryLoggingBootstrap : MonoBehaviour
{
    [SerializeField] private GameData gameData;

    private void Awake()
    {
        if (!gameData.telemetryLoggingBootstrapped)
        {
            GameTelemetryLogger.Initialize();
            gameData.telemetryLoggingBootstrapped = true;
        }
    }

    private void OnApplicationQuit()
    {
        GameTelemetryLogger.Shutdown();
    }
}
