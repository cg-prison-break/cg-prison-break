using System;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    [SerializeField] private GameData gameData;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameData.timer = 0.0f;
        PlayerCaughtEvent += IncrementStrikes;
    }
    
    void OnDestroy()
    {
        PlayerCaughtEvent -= IncrementStrikes;
    }

    // Update is called once per frame
    void Update()
    {
        gameData.timer += Time.deltaTime;
    }
    
    private void IncrementStrikes()
    {
        gameData.strikes++;
    }
    
    public static void PlayerCaught()
    {
        PlayerCaughtEvent?.Invoke();
    }
    
    static event Action PlayerCaughtEvent;
}
