using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseCanvas;
    [SerializeField] private GameObject pausePanel;          // root of Resume/Options/Quit UI
    [SerializeField] private GameObject optionsPanelInGame;  // separate panel for options
    [SerializeField] private string mainMenuSceneName = "MainMenu";
    [SerializeField] private string mainSceneName = "MainScene";
    [SerializeField] private MonoBehaviour[] scriptsToDisableOnPause;
    [SerializeField] private GameObject retryPanel;

    private bool _isPaused;
    public static bool InputsBlocked { get; private set; }

    private void Start()
    {
        if (pauseCanvas != null) pauseCanvas.SetActive(false);
        if (pausePanel != null) pausePanel.SetActive(false);
        if (optionsPanelInGame != null) optionsPanelInGame.SetActive(false);
        if (retryPanel != null) retryPanel.SetActive(false);

        _isPaused = false;
        InputsBlocked = false;
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
            else if (!retryPanel.activeSelf)
            {
                if (_isPaused) Resume();
                else Pause();
            }
        }
    }

    public void OpenRetryMenu()
    {
        if (retryPanel == null) return;

        pauseCanvas.SetActive(true);
        retryPanel.SetActive(true);
        Time.timeScale = 0f;
        _isPaused = true;
        InputsBlocked = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Disable gameplay scripts
        if (scriptsToDisableOnPause != null)
        {
            foreach (var s in scriptsToDisableOnPause)
            {
                if (s != null) s.enabled = false;
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
        InputsBlocked = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Disable gameplay scripts
        if (scriptsToDisableOnPause != null)
        {
            foreach (var s in scriptsToDisableOnPause)
            {
                if (s != null) s.enabled = false;
            }
        }
    }

    public void Resume()
    {
        if (pauseCanvas == null) return;

        pauseCanvas.SetActive(false);
        pausePanel.SetActive(false);
        retryPanel.SetActive(false);
        Time.timeScale = 1f;
        _isPaused = false;
        InputsBlocked = false;

        // Re-enable gameplay scripts
        if (scriptsToDisableOnPause != null)
        {
            foreach (var s in scriptsToDisableOnPause)
            {
                if (s != null) s.enabled = true;
            }
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
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
    
    public void Retry()
    {
        Time.timeScale = 1f;
        DataManager.PlayerCaught();
        SceneManager.LoadScene(mainSceneName);
    }

    public bool IsPaused => _isPaused;
}
