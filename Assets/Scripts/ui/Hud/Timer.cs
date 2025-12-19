using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] GameData gameData;
    [SerializeField] TMP_Text timerText;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        float time = gameData.timer;

        int minutes = Mathf.FloorToInt(time / 60f);
        float secondsFloat = time - minutes * 60;
        int seconds = Mathf.FloorToInt(secondsFloat);
        int centiseconds = Mathf.FloorToInt((secondsFloat - seconds) * 100f);
        centiseconds = Mathf.Clamp(centiseconds, 0, 99);

        timerText.text = $"{minutes:00}:{seconds:00}.{centiseconds:00}";
    }
}
