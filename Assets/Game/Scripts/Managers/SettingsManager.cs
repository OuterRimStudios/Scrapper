using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using Rewired;

public class SettingsManager : MonoBehaviour
{
    public Toggle fullscreenToggle;
    public Dropdown resolutionDropdown;
    public Dropdown textureQualityDropdown;
    public Dropdown antialiasingDropdown;
    public Dropdown vSyncDropdown;
    public Slider masterVolumeSlider;
    public Button applyButton;

    public Resolution[] resolutions;
    public GameSettings gameSettings;

    void OnEnable()
    {
        gameSettings = new GameSettings();
        fullscreenToggle.onValueChanged.AddListener(delegate { OnFullscreenToggle(); });
        resolutionDropdown.onValueChanged.AddListener(delegate { OnResolutionChange(); });
        textureQualityDropdown.onValueChanged.AddListener(delegate { OnTextureQualityChange(); });
        antialiasingDropdown.onValueChanged.AddListener(delegate { OnAntialiasingChange(); });
        vSyncDropdown.onValueChanged.AddListener(delegate { OnVSyncChange(); });
        masterVolumeSlider.onValueChanged.AddListener(delegate { OnVolumeChanged(masterVolumeSlider); });
        applyButton.onClick.AddListener(delegate { OnApplyButtonClicked(); });
        applyButton.interactable = false;
        resolutions = Screen.resolutions;
        foreach(Resolution resolution in resolutions)
        {
            resolutionDropdown.options.Add(new Dropdown.OptionData(resolution.ToString()));
        }

        LoadBindings();
        LoadSettings();
    }

    public void OnFullscreenToggle()
    {
        Screen.fullScreen = gameSettings.fullscreen = fullscreenToggle.isOn;
        applyButton.interactable = true;
    }

    public void OnResolutionChange()
    {
        Screen.SetResolution(resolutions[resolutionDropdown.value].width, resolutions[resolutionDropdown.value].height, Screen.fullScreen);
        gameSettings.resolutionIndex = resolutionDropdown.value;
        applyButton.interactable = true;
    }

    public void OnTextureQualityChange()
    {
        QualitySettings.masterTextureLimit = gameSettings.textureQuality = textureQualityDropdown.value;
        applyButton.interactable = true;
    }

    public void OnAntialiasingChange()
    {
        QualitySettings.antiAliasing = (int)Mathf.Pow(2, antialiasingDropdown.value);
        gameSettings.antialiasing = antialiasingDropdown.value;
        applyButton.interactable = true;
    }

    public void OnVSyncChange()
    {
        QualitySettings.vSyncCount = gameSettings.vSync = vSyncDropdown.value;
        applyButton.interactable = true;
    }

    public void OnVolumeChanged(Slider slider)
    {
        AudioListener.volume = gameSettings.masterVolume = slider.value;
        applyButton.interactable = true;
    }

    public void OnApplyButtonClicked()
    {
        SaveSettings();
        applyButton.interactable = false;
    }

    public void SaveBindings()
    {
        ReInput.userDataStore.Save();
    }

    public void LoadBindings()
    {
        ReInput.userDataStore.Load();
    }

    public void SaveSettings()
    {
        string jsonData = JsonUtility.ToJson(gameSettings, true);
        File.WriteAllText(Application.persistentDataPath + "/gamesettings.json", jsonData);
    }

    public void LoadSettings()
    {
        gameSettings = JsonUtility.FromJson<GameSettings>(File.ReadAllText(Application.persistentDataPath + "/gamesettings.json"));
        fullscreenToggle.isOn = gameSettings.fullscreen;
        resolutionDropdown.value = gameSettings.resolutionIndex;
        textureQualityDropdown.value = gameSettings.textureQuality;
        antialiasingDropdown.value = gameSettings.antialiasing;
        vSyncDropdown.value = gameSettings.vSync;
        masterVolumeSlider.value = gameSettings.masterVolume;

        Screen.fullScreen = gameSettings.fullscreen;
        resolutionDropdown.RefreshShownValue();
        applyButton.interactable = false;
    }
}

public class GameSettings
{
    public bool fullscreen;
    public int textureQuality;
    public int antialiasing;
    public int vSync;
    public int resolutionIndex;
    public float masterVolume;
}