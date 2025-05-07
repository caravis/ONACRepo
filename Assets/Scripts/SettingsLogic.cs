using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsLogic : MonoBehaviour
{

    public AudioMixer audioMixer;

    public Slider masterSlider;
    public Slider musicSlider;
    public Slider ambienceSlider;
    public Slider sfxSlider;


    void Start()
    {
        // Set up the event listeners for sliders
        masterSlider.onValueChanged.AddListener(SetMasterVolume);
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        ambienceSlider.onValueChanged.AddListener(SetAmbienceVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);

        // Load saved values or set default
        LoadVolume("Master", masterSlider);    // Match exposed parameter name
        LoadVolume("Music", musicSlider);      // Match exposed parameter name
        LoadVolume("Ambience", ambienceSlider); // Match exposed parameter name
        LoadVolume("Sound FX", sfxSlider);     // Match exposed parameter name
    }

    #region AUDIO
    public void SetMasterVolume(float volume)
    {
        SetVolume("Master", volume); // Ensure the name matches the exposed parameter
    }

    public void SetMusicVolume(float volume)
    {
        SetVolume("Music", volume); // Ensure the name matches the exposed parameter
    }

    public void SetAmbienceVolume(float volume)
    {
        SetVolume("Ambience", volume); // Ensure the name matches the exposed parameter
    }

    public void SetSFXVolume(float volume)
    {
        SetVolume("Sound FX", volume); // Ensure the name matches the exposed parameter
    }

    private void SetVolume(string parameterName, float sliderValue)
    {
        // Convert slider value to decibels
        float db = Mathf.Log10(Mathf.Clamp(sliderValue, 0.0001f, 1f)) * 20;

        // Set the exposed parameter in the Audio Mixer
        audioMixer.SetFloat(parameterName, db);

        // Debug log to check if parameter is being set
        Debug.Log($"Setting {parameterName} to {db} dB");

        // Save the slider value to PlayerPrefs
        PlayerPrefs.SetFloat(parameterName, sliderValue);

        // Debug to confirm that PlayerPrefs is saving correctly
        float savedVolume = PlayerPrefs.GetFloat(parameterName);
        Debug.Log($"Saved {parameterName}: {savedVolume}");
    }

    private void LoadVolume(string parameterName, Slider slider)
    {
        float value = PlayerPrefs.GetFloat(parameterName, 0.75f); // Default to 75%
        slider.value = value;
        SetVolume(parameterName, value);

        // Debug to check if loading works
        Debug.Log($"Loaded {parameterName} from PlayerPrefs: {value}");
    }
    #endregion
}
