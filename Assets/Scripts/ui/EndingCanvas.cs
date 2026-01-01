using System.Linq;
using TMPro;
using Unity.AppUI.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingCanvas : MonoBehaviour
{
    [SerializeField] private Button victoryButton;
    [SerializeField] private Button defeatButton;
    [SerializeField] private Button retryButton;
    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private GameObject defeatPanel;
    
    [SerializeField] private GameData gameData;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (victoryPanel != null) victoryPanel.SetActive(false);
        if (defeatPanel != null) defeatPanel.SetActive(false);

        ShowEnding(EndingContext.NextEnding);
        
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void ShowEnding(EndingType type)
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        bool good = type == EndingType.Good;
        GameObject endingPanel = good ? victoryPanel : defeatPanel;
        
        float time = gameData.timer;
        int minutes = Mathf.FloorToInt(time / 60f);
        float secondsFloat = time - minutes * 60;
        int seconds = Mathf.FloorToInt(secondsFloat);
        int centiseconds = Mathf.FloorToInt((secondsFloat - seconds) * 100f);
        centiseconds = Mathf.Clamp(centiseconds, 0, 99);
        
        TMP_Text[] allTexts = endingPanel.GetComponentsInChildren<TMP_Text>();
        TMP_Text endingTime = allTexts.FirstOrDefault(t => t.gameObject.name == "TimeText");
        
        string minutesPart = minutes > 0 ? $"{minutes}:" : "";
        endingTime.text = $"Finale Zeit: {minutesPart}{seconds:00}.{centiseconds:00}";
        
        victoryPanel.SetActive(good);
        defeatPanel.SetActive(!good);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(GameScene.MainMenu);
    }
}
