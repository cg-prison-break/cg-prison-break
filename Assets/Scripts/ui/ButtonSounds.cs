using UnityEngine;
using UnityEngine.UI;

// [RequireComponent(typeof(Button))]
public class ButtonSounds : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip clickClip;
    [SerializeField, Range(0f, 1f)] private float volume = 1f;

    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    private void OnEnable()
    {
        button.onClick.AddListener(PlayClick);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(PlayClick);
    }

    private void PlayClick()
    {
        if (clickClip == null)
        {
            return;
        }

        if (audioSource == null)
        {
            Debug.LogWarning("ButtonSounds: No AudioSource assigned or found.", this);
            return;
        }

        audioSource.PlayOneShot(clickClip, volume);
    }
}
