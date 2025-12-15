// EscapeRouteDetailPanel.cs
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EscapeRouteDetailPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text bodyText;
    [SerializeField] private RawImage routeImage;
    [SerializeField] private GameObject root;

    public void Show(string title, string description, Texture image)
    {
        if (!string.IsNullOrEmpty(title) && titleText != null)
        {
            titleText.text = title;
        }
        else
        {
            titleText.text = "Unknown Route";
        }

        if (!string.IsNullOrEmpty(description) && bodyText != null)
        {
            bodyText.text = description;
        }
        else
        {
            bodyText.text = "No description available.";
        }

        if (routeImage != null)
        {
            routeImage.texture = image;
        }

        root.SetActive(true);
    }

    public void Hide() => root.SetActive(false);
}
