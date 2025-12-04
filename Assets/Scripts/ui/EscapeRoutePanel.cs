using UnityEngine;
using UnityEngine.InputSystem;

public class EscapeRoutePanel : MonoBehaviour
{
    [SerializeField] private GameObject escapeRoutePanel;
    [SerializeField] private GameObject interactPanel;
    [SerializeField] private GameObject itemPanel;
    [SerializeField] private GameObject crosshair;

    private bool isActive;

    private void Start()
    {
        SetEscapeRoutePanelActive(false);
    }

    private void Update()
    {
        var keyboard = Keyboard.current;
        if (keyboard == null) return;

        if (keyboard.mKey.wasPressedThisFrame)
        {
            SetEscapeRoutePanelActive(true);
        }
        else if (keyboard.escapeKey.wasPressedThisFrame || keyboard.nKey.wasPressedThisFrame)
        {
            SetEscapeRoutePanelActive(false);
        }
    }

    private void SetEscapeRoutePanelActive(bool active)
    {
        if (isActive == active) return;

        isActive = active;
        escapeRoutePanel?.SetActive(active);
        interactPanel?.SetActive(!active);
        itemPanel?.SetActive(!active);
        crosshair?.SetActive(!active);
    }
}
