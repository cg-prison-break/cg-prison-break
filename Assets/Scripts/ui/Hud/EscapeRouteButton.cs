using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class EscapeRouteButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private EscapeRoutePanel escapeRoutePanel;
    // [SerializeField] private string routeId;
    [SerializeField] private string title;
    [SerializeField] [TextArea] private string description;
    [SerializeField] private Texture routeImage;

    private RawImage rawImage;

    private void Awake()
    {
        rawImage = GetComponent<RawImage>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (escapeRoutePanel == null)
        {
            Debug.LogWarning($"{nameof(EscapeRouteButton)} on {name} is missing EscapeRoutePanel reference.");
            return;
        }

        var imageToSend = routeImage != null ? routeImage : rawImage != null ? rawImage.texture : null;
        escapeRoutePanel.ShowRouteDetail(title, description, imageToSend);
    }
}
