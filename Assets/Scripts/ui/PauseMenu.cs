using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseCanvas;

    private bool _isPaused;

    private void Start()
    {
        if (pauseCanvas != null)
            pauseCanvas.SetActive(false);

        _isPaused = false;
        Time.timeScale = 1f;
    }

    private void Update()
    {
        Debug.Log("PauseMenu.Update running");

        if (Keyboard.current == null)
            return;

        // Escape or P to toggle pause
        if (Keyboard.current.escapeKey.wasPressedThisFrame ||
            Keyboard.current.pKey.wasPressedThisFrame)
        {
            Debug.Log("Pause key pressed");

            if (_isPaused)
                Resume();
            else
                Pause();
        }
    }

    public void Pause()
    {
        if (pauseCanvas == null) return;

        pauseCanvas.SetActive(true);
        Time.timeScale = 0f;   // freeze game
        _isPaused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Resume()
    {
        if (pauseCanvas == null) return;

        pauseCanvas.SetActive(false);
        Time.timeScale = 1f;   // unfreeze game
        _isPaused = false;
        // If you lock the cursor in your game, re-lock here:
        // Cursor.lockState = CursorLockMode.Locked;
        // Cursor.visible = false;
    }

    // We'll wire these later:
    public void QuitToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu"); // change name if needed
    }
}