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

    private Resolution[] _resolutions;

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
        _resolutions = Screen.resolutions;
        var options = new List<string>();
        var seen = new HashSet<string>();
        int currentIndex = 0;

        resolutionDropdown.ClearOptions();

        for (int i = 0; i < _resolutions.Length; i++)
        {
            Resolution r = _resolutions[i];
            string key = $"{r.width}x{r.height}";
            if (seen.Add(key))
            {
                options.Add($"{r.width} x {r.height}");
                if (r.width == Screen.currentResolution.width && r.height == Screen.currentResolution.height)
                    currentIndex = options.Count - 1;
            }
        }

        resolutionDropdown.AddOptions(options);
        _currentResIndex = PlayerPrefs.GetInt(PREF_RES_INDEX, currentIndex);
        resolutionDropdown.value = Mathf.Clamp(_currentResIndex, 0, options.Count - 1);
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

        ApplyResolution(_currentResIndex, savedFullscreen);
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
        if (index < 0 || index >= _resolutions.Length) return;

        Resolution chosen = _resolutions[index];
        Screen.SetResolution(chosen.width, chosen.height, fullscreen);
    }

    private void ApplyAudioImmediate(float db)
    {
        if (masterMixer)
            masterMixer.SetFloat("MasterVol", db);
    }
}