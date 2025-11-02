using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class OptionsMenu : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioMixer masterMixer;   // Expose "MasterVol" in mixer

    [Header("Display")]
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private Toggle fullscreenToggle;

    [Header("Audio UI")]
    [SerializeField] private Slider masterVolumeSlider;

    private Resolution[] _resolutions;
    private const string PREF_RES_INDEX = "res_index";
    private const string PREF_FULLSCREEN = "fullscreen";
    private const string PREF_MASTER_DB = "master_db";

    void Awake()
    {
        // Populate resolutions (unique width×height)
        _resolutions = Screen.resolutions;
        var options = new List<string>();
        int currentIndex = 0;
        var seen = new HashSet<string>();

        resolutionDropdown.ClearOptions();

        for (int i = 0; i < _resolutions.Length; i++)
        {
            string label = $"{_resolutions[i].width} x {_resolutions[i].height} @ {_resolutions[i].refreshRateRatio.value:0}Hz";
            string key = $"{_resolutions[i].width}x{_resolutions[i].height}";
            if (seen.Add(key))
            {
                options.Add(label);
                if (_resolutions[i].width == Screen.currentResolution.width &&
                    _resolutions[i].height == Screen.currentResolution.height)
                {
                    currentIndex = options.Count - 1;
                }
            }
        }

        resolutionDropdown.AddOptions(options);

        // Load saved prefs or defaults
        int savedResIndex = PlayerPrefs.GetInt(PREF_RES_INDEX, currentIndex);
        bool savedFullscreen = PlayerPrefs.GetInt(PREF_FULLSCREEN, Screen.fullScreen ? 1 : 0) == 1;
        float savedMasterDb = PlayerPrefs.GetFloat(PREF_MASTER_DB, 0f); // 0 dB default

        resolutionDropdown.value = Mathf.Clamp(savedResIndex, 0, options.Count - 1);
        resolutionDropdown.RefreshShownValue();

        fullscreenToggle.isOn = savedFullscreen;

        if (masterVolumeSlider)
        {
            // Convert dB to 0..1 slider roughly; store slider as linear 0..1, convert to dB on apply
            // We’ll map slider [0.0001..1] to dB = 20*log10(x). Rebuild from saved dB:
            float lin = Mathf.Pow(10f, savedMasterDb / 20f);
            masterVolumeSlider.value = Mathf.Clamp01(lin);
        }

        // Apply immediately so UI reflects actual state
        ApplyAudioImmediate(savedMasterDb);
    }

    public void OnMasterVolumeChanged(float linear01)
    {
        // Don’t set prefs yet; wait for Apply
        float clamped = Mathf.Clamp(linear01, 0.0001f, 1f);
        float db = 20f * Mathf.Log10(clamped);
        masterMixer.SetFloat("MasterVol", db);
    }

    public void Apply()
    {
        // Resolution & fullscreen
        int resIndex = resolutionDropdown.value;
        // Map the dropdown index back to a Resolution choice:
        // For simplicity, rebuild a unique list again (same as Awake)
        var unique = new List<Resolution>();
        var seen = new HashSet<string>();
        foreach (var r in Screen.resolutions)
        {
            string key = $"{r.width}x{r.height}";
            if (seen.Add(key)) unique.Add(r);
        }
        resIndex = Mathf.Clamp(resIndex, 0, unique.Count - 1);
        var chosen = unique[resIndex];
        Screen.SetResolution(chosen.width, chosen.height, fullscreenToggle.isOn);

        PlayerPrefs.SetInt(PREF_RES_INDEX, resIndex);
        PlayerPrefs.SetInt(PREF_FULLSCREEN, fullscreenToggle.isOn ? 1 : 0);

        // Audio
        float lin = masterVolumeSlider ? Mathf.Clamp(masterVolumeSlider.value, 0.0001f, 1f) : 1f;
        float db = 20f * Mathf.Log10(lin);
        ApplyAudioImmediate(db);
        PlayerPrefs.SetFloat(PREF_MASTER_DB, db);

        PlayerPrefs.Save();
    }

    public void Back(MainMenu menu)
    {
        menu.CloseOptions();
    }

    private void ApplyAudioImmediate(float db)
    {
        if (masterMixer) masterMixer.SetFloat("MasterVol", db);
    }
}