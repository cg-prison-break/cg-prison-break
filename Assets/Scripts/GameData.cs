using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "Persistent Data/GameData")]
public class GameData : ScriptableObject
{
    public int strikes;
    public float timer = 0.0f;
    public bool animationPlayed = false;
    public List<string> collectedItems = new List<string>();
    public bool telemetryLoggingBootstrapped = false;

    public bool playWithInteractableShader = true;

}
