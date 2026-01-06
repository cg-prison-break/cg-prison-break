using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class OptionsMenu : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioMixer masterMixer;   // Expose "MasterVol" in mixer
    [SerializeField] private Slider masterVolumeSlider;

    [Header("Display")]
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private Toggle fullscreenToggle;

    private readonly List<Vector2Int> _filteredResolutions = new List<Vector2Int>();

    private const string PREF_RES_INDEX = "res_index";
    private const string PREF_FULLSCREEN = "fullscreen";
    private const string PREF_MASTER_DB = "master_db";

    private int _currentResIndex;

    void Awake()
    {
        PopulateResolutions();
        LoadSettings();
        ApplyAudioImmediate(PlayerPrefs.GetFloat(PREF_MASTER_DB, 0f));

        // Hook up listeners so changes can apply in real time if you like
        if (resolutionDropdown)
            resolutionDropdown.onValueChanged.AddListener(OnResolutionChanged);
        if (fullscreenToggle)
            fullscreenToggle.onValueChanged.AddListener(OnFullscreenToggled);
        if (masterVolumeSlider)
            masterVolumeSlider.onValueChanged.AddListener(OnMasterVolumeChanged);
    }

    // --- Resolution Picker Setup ---
    private void PopulateResolutions()
    {
        var options = new List<string>();
        var seen = new HashSet<string>();
        int currentIndex = 0;

        resolutionDropdown.ClearOptions();
        _filteredResolutions.Clear();

        Vector2Int[] fixedResolutions =
        {
            new Vector2Int(1280, 720),
            new Vector2Int(1366, 768),
            new Vector2Int(1600, 900),
            new Vector2Int(1920, 1080),
            new Vector2Int(2560, 1440),
            new Vector2Int(3840, 2160)
        };

        for (int i = 0; i < fixedResolutions.Length; i++)
        {
            Vector2Int r = fixedResolutions[i];
            string key = $"{r.x}x{r.y}";
            if (seen.Add(key))
            {
                options.Add($"{r.x} x {r.y}");
                _filteredResolutions.Add(r);
                if (r.x == Screen.currentResolution.width && r.y == Screen.currentResolution.height)
                    currentIndex = options.Count - 1;
            }
        }

        resolutionDropdown.AddOptions(options);
        if (options.Count == 0)
        {
            _currentResIndex = 0;
            resolutionDropdown.value = 0;
            resolutionDropdown.RefreshShownValue();
            return;
        }

        _currentResIndex = Mathf.Clamp(PlayerPrefs.GetInt(PREF_RES_INDEX, currentIndex), 0, options.Count - 1);
        resolutionDropdown.value = _currentResIndex;
        resolutionDropdown.RefreshShownValue();
    }

    // --- Load saved settings ---
    private void LoadSettings()
    {
        bool savedFullscreen = PlayerPrefs.GetInt(PREF_FULLSCREEN, Screen.fullScreen ? 1 : 0) == 1;
        float savedMasterDb = PlayerPrefs.GetFloat(PREF_MASTER_DB, 0f);

        fullscreenToggle.isOn = savedFullscreen;

        if (masterVolumeSlider)
        {
            float lin = Mathf.Pow(10f, savedMasterDb / 20f);
            masterVolumeSlider.value = Mathf.Clamp01(lin);
        }

    }

    // --- Callbacks ---
    private void OnResolutionChanged(int index)
    {
        _currentResIndex = index;
        ApplyResolution(_currentResIndex, fullscreenToggle.isOn);
    }

    private void OnFullscreenToggled(bool isFullscreen)
    {
        ApplyResolution(_currentResIndex, isFullscreen);
    }

    public void OnMasterVolumeChanged(float linear01)
    {
        float clamped = Mathf.Clamp(linear01, 0.0001f, 1f);
        float db = 20f * Mathf.Log10(clamped);
        masterMixer.SetFloat("MasterVol", db);
    }

    // --- Apply and Save ---
    public void Apply()
    {
        bool isFullscreen = fullscreenToggle.isOn;
        ApplyResolution(_currentResIndex, isFullscreen);

        // Save everything
        PlayerPrefs.SetInt(PREF_RES_INDEX, _currentResIndex);
        PlayerPrefs.SetInt(PREF_FULLSCREEN, isFullscreen ? 1 : 0);

        float lin = Mathf.Clamp(masterVolumeSlider.value, 0.0001f, 1f);
        float db = 20f * Mathf.Log10(lin);
        PlayerPrefs.SetFloat(PREF_MASTER_DB, db);
        ApplyAudioImmediate(db);

        PlayerPrefs.Save();
    }

    public void Back(MainMenu menu)
    {
        menu.CloseOptions();
    }

    // --- Helpers ---
    private void ApplyResolution(int index, bool fullscreen)
    {
        if (index < 0 || index >= _filteredResolutions.Count) return;

        Vector2Int chosen = _filteredResolutions[index];
        Screen.SetResolution(chosen.x, chosen.y, fullscreen);
    }

    private void ApplyAudioImmediate(float db)
    {
        if (masterMixer)
            masterMixer.SetFloat("MasterVol", db);
    }
}
