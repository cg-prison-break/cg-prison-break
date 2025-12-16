using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Unity.AppUI.UI;

public class InteractPromptUI : MonoBehaviour
{
    [SerializeField] private TMP_Text promptText;
    [SerializeField] private GameObject panel;
    [SerializeField] private GameObject escapeRoutePanel;
    [SerializeField] private EscapeRouteDetailPanel escapeRouteDetailPanel;
    [SerializeField] private RectTransform panelRect;
    [SerializeField] private Vector2 padding = new Vector2(24f, 12f);
    [SerializeField] private Vector2 minSize = new Vector2(100f, 40f);

    private void Awake()
    {
        if (promptText == null)
            promptText = GetComponentInChildren<TMP_Text>(true);

        if (panelRect == null && panel != null)
            panelRect = panel.GetComponent<RectTransform>();

        Hide();
    }

    public void Show(string prompt)
    {
        if (string.IsNullOrWhiteSpace(prompt) || escapeRoutePanel.activeSelf || escapeRouteDetailPanel.gameObject.activeSelf)
        {
            Hide();
            return;
        }

        promptText.text = $"{prompt}";
        AdjustPanelSize(prompt);
        panel.SetActive(true);
    }

    public void Hide()
    {
        if (promptText != null)
            promptText.text = string.Empty;

        panel.SetActive(false);
    }

    private void AdjustPanelSize(string prompt)
    {
        if (panelRect == null || promptText == null)
            return;

        // Calculate preferred text size and add padding for comfortable spacing.
        var preferred = promptText.GetPreferredValues(prompt);
        var newSize = preferred + padding;

        newSize.x = Mathf.Max(newSize.x, minSize.x);
        newSize.y = Mathf.Max(newSize.y, minSize.y);

        panelRect.sizeDelta = newSize;
        promptText.rectTransform.sizeDelta = newSize;
        LayoutRebuilder.ForceRebuildLayoutImmediate(panelRect);
    }
}
