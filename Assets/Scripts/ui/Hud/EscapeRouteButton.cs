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
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip clickClip;
    [SerializeField, Range(0f, 1f)] private float volume = 1f;

    private RawImage rawImage;

    private void Awake()
    {
        rawImage = GetComponent<RawImage>();
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (escapeRoutePanel == null)
        {
            Debug.LogWarning($"{nameof(EscapeRouteButton)} on {name} is missing EscapeRoutePanel reference.");
            return;
        }

        PlayClick();
        var imageToSend = routeImage != null ? routeImage : rawImage != null ? rawImage.texture : null;
        escapeRoutePanel.ShowRouteDetail(title, description, imageToSend);
    }

    private void PlayClick()
    {
        if (clickClip == null || audioSource == null)
        {
            return;
        }

        audioSource.PlayOneShot(clickClip, volume);
    }
}
