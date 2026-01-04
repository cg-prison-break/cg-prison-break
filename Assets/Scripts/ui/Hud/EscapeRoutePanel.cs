using UnityEngine;
using UnityEngine.InputSystem;

public class EscapeRoutePanel : MonoBehaviour
{
    [SerializeField] private GameObject escapeRoutePanel;
    [SerializeField] private GameObject interactPanel;
    [SerializeField] private GameObject itemPanel;
    [SerializeField] private GameObject crosshair;
    [SerializeField] private GameObject playerObject;
    [SerializeField] private EscapeRouteDetailPanel detailPanel;
    [SerializeField] private GameObject minimap;
    [SerializeField] private GameObject timerPanel;
    [SerializeField] private GameObject strikePanel;
    private PlayerInput playerInput;
    private PlayerController playerController;

    private bool isActive;
    private bool playerInputWasEnabled;
    private CursorLockMode previousLockMode;
    private bool previousCursorVisible;

    private void Start()
    {
        CachePlayerComponents();

        isActive = false;
        escapeRoutePanel?.SetActive(false);
        detailPanel?.Hide();
    }

    private void Update()
    {
        var keyboard = Keyboard.current;
        if (keyboard == null) return;
        if (PauseMenu.InputsBlocked) return;

        if (keyboard.mKey.wasPressedThisFrame && !isActive)
        {
            SetEscapeRoutePanelActive(true);
        }
        else if (keyboard.escapeKey.wasPressedThisFrame || keyboard.mKey.wasPressedThisFrame && isActive)
        {
            SetEscapeRoutePanelActive(false);
        }
    }

    private void SetEscapeRoutePanelActive(bool active)
    {
        if (isActive == active) return;

        isActive = active;
        escapeRoutePanel?.SetActive(active);
        detailPanel?.Hide();
        interactPanel?.SetActive(!active);
        itemPanel?.SetActive(!active);
        crosshair?.SetActive(!active);
        minimap?.SetActive(!active);
        timerPanel?.SetActive(!active);
        strikePanel?.SetActive(!active);

        if (active)
        {
            DisablePlayerControl();
            ShowCursor();
            GameTelemetryLogger.LogTelemetryEvent(new SpreadsheetOpenedData());
        }
        else
        {
            EnablePlayerControl();
            RestoreCursor();
            GameTelemetryLogger.LogTelemetryEvent(new SpreadsheetClosedData());
        }
    }

    private void DisablePlayerControl()
    {
        if (playerInput != null)
        {
            playerInputWasEnabled = playerInput.enabled;
            playerInput.enabled = false;
        }

        // Clear any lingering input so movement stops immediately.
        if (playerController != null)
        {
            playerController.MoveInput(Vector2.zero);
            playerController.LookInput(Vector2.zero);
            playerController.SprintInput(false);
            playerController.JumpInput(false);
        }
    }

    private void EnablePlayerControl()
    {
        if (playerInput != null && playerInputWasEnabled)
        {
            playerInput.enabled = true;
        }
    }

    private void ShowCursor()
    {
        previousLockMode = Cursor.lockState;
        previousCursorVisible = Cursor.visible;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void RestoreCursor()
    {
        Cursor.lockState = previousLockMode;
        Cursor.visible = previousCursorVisible;
    }

    public void ShowRouteDetail(string title, string description, Texture image)
    {
        if (!isActive)
        {
            SetEscapeRoutePanelActive(true);
        }

        escapeRoutePanel?.SetActive(false);
        detailPanel?.Show(title, description, image);
        GameTelemetryLogger.LogTelemetryEvent(new SpreadsheetRouteClickedData(title));
    }

    public void ShowRoutesList()
    {
        detailPanel?.Hide();
        escapeRoutePanel?.SetActive(true);
    }

    private void CachePlayerComponents()
    {
        if (playerObject == null)
        {
            Debug.LogWarning($"{nameof(EscapeRoutePanel)} missing playerObject reference; movement disabling will be skipped.");
            return;
        }

        playerInput = playerObject.GetComponent<PlayerInput>();
        playerController = playerObject.GetComponent<PlayerController>();
    }
}
