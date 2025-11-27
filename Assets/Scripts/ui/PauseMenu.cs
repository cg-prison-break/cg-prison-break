using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseCanvas;
    [SerializeField] private GameObject pausePanel;          // root of Resume/Options/Quit UI
    [SerializeField] private GameObject optionsPanelInGame;  // separate panel for options
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    private bool _isPaused;

    private void Start()
    {
        if (pauseCanvas != null) pauseCanvas.SetActive(false);
        if (pausePanel != null) pausePanel.SetActive(true);
        if (optionsPanelInGame != null) optionsPanelInGame.SetActive(false);

        _isPaused = false;
        Time.timeScale = 1f;
    }

    private void Update()
    {
        if (Keyboard.current == null) return;

        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            // If options are open, close them first on Escape
            if (_isPaused && optionsPanelInGame != null && optionsPanelInGame.activeSelf)
            {
                CloseOptions();
            }
            else
            {
                if (_isPaused) Resume();
                else Pause();
            }
        }
    }

    public void Pause()
    {
        if (pauseCanvas == null) return;

        pauseCanvas.SetActive(true);
        pausePanel.SetActive(true);
        if (optionsPanelInGame != null) optionsPanelInGame.SetActive(false);

        Time.timeScale = 0f;
        _isPaused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Resume()
    {
        if (pauseCanvas == null) return;

        pauseCanvas.SetActive(false);
        Time.timeScale = 1f;
        _isPaused = false;
        // re-lock cursor here if your game normally does that
    }

    public void OpenOptions()
    {
        if (pausePanel != null) pausePanel.SetActive(false);
        if (optionsPanelInGame != null) optionsPanelInGame.SetActive(true);
    }

    public void CloseOptions()
    {
        if (optionsPanelInGame != null) optionsPanelInGame.SetActive(false);
        if (pausePanel != null) pausePanel.SetActive(true);
    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuSceneName);
    }

    public bool IsPaused => _isPaused;
}