using System;
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
    [SerializeField] private string mainMenuSceneName = "MainMenu";
    
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
        bool good = type == EndingType.Good;
        victoryPanel.SetActive(good);
        defeatPanel.SetActive(!good);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
