using Unity.AppUI.Core;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "Persistent Data/GameData")]
public class GameData : ScriptableObject
{
    public int strikes;
    
    
    public float timer = 0.0f;

    public bool animationPlayed = false;

}
