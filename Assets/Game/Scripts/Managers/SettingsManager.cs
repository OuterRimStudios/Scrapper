using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using Rewired;

public class SettingsManager : MonoBehaviour
{
    [Header("Gameplay Settings")]
    public Slider mouseSensitivitySlider;
    public Slider controllerSensitivitySlider;
    public Toggle firstPersonToggle;
    public Toggle thirdPersonToggle;
    public Button gameplayApplyButton;

    [Header("Video Settings")]
    public Toggle fullscreenToggle;
    public Dropdown resolutionDropdown;
    public Dropdown textureQualityDropdown;
    public Dropdown antialiasingDropdown;
    public Dropdown vSyncDropdown;
    public Button videoApplyButton;

    [Header("Audio Settings")]
    public Slider masterVolumeSlider;

    public Resolution[] resolutions;
    public GameSettings gameSettings;

    InputBehavior inputBehavior;

    private void Awake()
    {
        inputBehavior = ReInput.mapping.GetInputBehavior(0, 0);
    }

    void OnEnable()
    {
        gameSettings = new GameSettings();

        mouseSensitivitySlider.onValueChanged.AddListener(delegate { OnMouseSensitivityChanged(); });
        controllerSensitivitySlider.onValueChanged.AddListener(delegate { OnControllerSensitivityChanged(); });
        firstPersonToggle.onValueChanged.AddListener(delegate { OnDefaultViewChanged(); });

        fullscreenToggle.onValueChanged.AddListener(delegate { OnFullscreenToggle(); });
        resolutionDropdown.onValueChanged.AddListener(delegate { OnResolutionChange(); });
        textureQualityDropdown.onValueChanged.AddListener(delegate { OnTextureQualityChange(); });
        antialiasingDropdown.onValueChanged.AddListener(delegate { OnAntialiasingChange(); });
        vSyncDropdown.onValueChanged.AddListener(delegate { OnVSyncChange(); });

        masterVolumeSlider.onValueChanged.AddListener(delegate { OnVolumeChanged(masterVolumeSlider); });

        videoApplyButton.onClick.AddListener(delegate { OnApplyButtonClicked(); });
        gameplayApplyButton.onClick.AddListener(delegate { OnApplyButtonClicked(); });
        videoApplyButton.interactable = false;
        gameplayApplyButton.interactable = false;
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
        videoApplyButton.interactable = true;
    }

    public void OnResolutionChange()
    {
        Screen.SetResolution(resolutions[resolutionDropdown.value].width, resolutions[resolutionDropdown.value].height, Screen.fullScreen);
        gameSettings.resolutionIndex = resolutionDropdown.value;
        videoApplyButton.interactable = true;
    }

    public void OnTextureQualityChange()
    {
        QualitySettings.masterTextureLimit = gameSettings.textureQuality = textureQualityDropdown.value;
        videoApplyButton.interactable = true;
    }

    public void OnAntialiasingChange()
    {
        QualitySettings.antiAliasing = (int)Mathf.Pow(2, antialiasingDropdown.value);
        gameSettings.antialiasing = antialiasingDropdown.value;
        videoApplyButton.interactable = true;
    }

    public void OnVSyncChange()
    {
        QualitySettings.vSyncCount = gameSettings.vSync = vSyncDropdown.value;
        videoApplyButton.interactable = true;
    }

    public void OnMouseSensitivityChanged()
    {
        inputBehavior.mouseXYAxisSensitivity = gameSettings.mouseSensitivity = mouseSensitivitySlider.value;
        gameplayApplyButton.interactable = true;
    }

    public void OnControllerSensitivityChanged()
    {
        inputBehavior.joystickAxisSensitivity = gameSettings.controllerSensitivity = controllerSensitivitySlider.value;
        gameplayApplyButton.interactable = true;
    }

    public void OnDefaultViewChanged()
    {
        CameraController.firstPerson = gameSettings.firstPerson = firstPersonToggle.isOn;
        PlayerReferenceManager.firstPerson = gameSettings.firstPerson;
        gameplayApplyButton.interactable = true;
    }

    void SetDefaultViewToggle()
    {
        if(gameSettings.firstPerson)
            firstPersonToggle.isOn = gameSettings.firstPerson;
        else
            thirdPersonToggle.isOn = !gameSettings.firstPerson;
    }

    public void OnVolumeChanged(Slider slider)
    {
        AudioListener.volume = gameSettings.masterVolume = slider.value;
        videoApplyButton.interactable = true;
    }

    public void OnApplyButtonClicked()
    {
        SaveSettings();
        videoApplyButton.interactable = false;
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

        if (gameSettings == null) return;

        mouseSensitivitySlider.value = gameSettings.mouseSensitivity;
        controllerSensitivitySlider.value = gameSettings.controllerSensitivity;
        SetDefaultViewToggle();

        fullscreenToggle.isOn = gameSettings.fullscreen;
        resolutionDropdown.value = gameSettings.resolutionIndex;
        textureQualityDropdown.value = gameSettings.textureQuality;
        antialiasingDropdown.value = gameSettings.antialiasing;
        vSyncDropdown.value = gameSettings.vSync;

        masterVolumeSlider.value = gameSettings.masterVolume;

        Screen.fullScreen = gameSettings.fullscreen;
        resolutionDropdown.RefreshShownValue();
        videoApplyButton.interactable = false;
        gameplayApplyButton.interactable = false;
    }
}

public class GameSettings
{
    //gameplay settings
    public float mouseSensitivity;
    public float controllerSensitivity;
    public bool firstPerson;

    //video settings
    public bool fullscreen;
    public int textureQuality;
    public int antialiasing;
    public int vSync;
    public int resolutionIndex;

    //audio settings
    public float masterVolume;
}