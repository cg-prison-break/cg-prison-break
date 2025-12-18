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
        timerText.text = gameData.timer.ToString("F2");
    }
}
