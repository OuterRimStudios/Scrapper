using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;
using Rewired;
using System;
using TMPro;

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
    [Header("Resolution")]
    public TextMeshProUGUI resolutionText;
    public Resolution[] resolutions;
    [HideInInspector] public List<string> resolutionOptions;
    public Button resLeftArrow;
    public Button resRightArrow;
    int currentRes;
    [Header("Texture Quality")]
    public TextMeshProUGUI textureQualityText;
    public List<string> textureQualityOptions;
    public Button texLeftArrow;
    public Button texRightArrow;
    int currentTex;
    [Header("Antialiasing")]
    public TextMeshProUGUI antialiasingText;
    public List<string> antialiasingOptions;
    public Button antLeftArrow;
    public Button antRightArrow;
    int currentAnt;
    [Header("VSync")]
    public TextMeshProUGUI vSyncText;
    public List<string> vSyncOptions;
    public Button vsyLeftArrow;
    public Button vsyRightArrow;
    int currentVsy;

    int currentlySelected = -1;
    bool cyclingSetting;
    WaitForSeconds cycleSpeed = new WaitForSeconds(.25f);

    [Space]
    public Button videoApplyButton;

    [Header("Audio Settings")]
    public Slider masterVolumeSlider;
    
    public GameSettings gameSettings;

    InputBehavior inputBehavior;
    Player player;
    EventSystem es;

    private void Awake()
    {
        inputBehavior = ReInput.mapping.GetInputBehavior(0, 0);
        player = ReInput.players.GetPlayer(0);
        es = EventSystem.current;
    }

    void OnEnable()
    {
        gameSettings = new GameSettings();

        resolutions = Screen.resolutions;
        foreach(Resolution resolution in resolutions)
        {
            resolutionOptions.Add(resolution.ToString());
        }

        mouseSensitivitySlider.onValueChanged.AddListener(delegate { OnMouseSensitivityChanged(); });
        controllerSensitivitySlider.onValueChanged.AddListener(delegate { OnControllerSensitivityChanged(); });
        firstPersonToggle.onValueChanged.AddListener(delegate { OnDefaultViewChanged(); });

        fullscreenToggle.onValueChanged.AddListener(delegate { OnFullscreenToggle(); });

        resLeftArrow.onClick.AddListener(delegate { UpdateResolution(-1); });
        resRightArrow.onClick.AddListener(delegate { UpdateResolution(1); });

        texLeftArrow.onClick.AddListener(delegate { UpdateTextureQuality(-1); });
        texRightArrow.onClick.AddListener(delegate { UpdateTextureQuality(1); });

        antLeftArrow.onClick.AddListener(delegate { UpdateAntialiasing(-1); });
        antRightArrow.onClick.AddListener(delegate { UpdateAntialiasing(1); });

        vsyLeftArrow.onClick.AddListener(delegate { UpdateVSync(-1); });
        vsyRightArrow.onClick.AddListener(delegate { UpdateVSync(1); });

        masterVolumeSlider.onValueChanged.AddListener(delegate { OnVolumeChanged(masterVolumeSlider); });

        videoApplyButton.onClick.AddListener(delegate { OnApplyButtonClicked(); });
        gameplayApplyButton.onClick.AddListener(delegate { OnApplyButtonClicked(); });
        videoApplyButton.interactable = false;
        gameplayApplyButton.interactable = false;

        LoadBindings();
        LoadSettings();
    }

    void Update()
    {
        //if(player.controllers.joystickCount > 0)
        //    es.sendNavigationEvents = true;
        //else
        //    es.sendNavigationEvents = false;
        
        if(currentlySelected == -1) return;

        int direction = 0;
        float inputValue = player.GetAxis("UIHorizontal");

        if(inputValue < -0.5f)
            direction = -1;
        else if(inputValue > 0.5f)
            direction = 1;
        else
            return;

        StartCoroutine(CycleSetting(direction));
    }

    public void SetCurrentlySelected(int index)
    {
        currentlySelected = index;
    }

    IEnumerator CycleSetting(int direction)
    {
        if(!cyclingSetting)
        {
            cyclingSetting = true;

            switch(currentlySelected)
            {
                case 0:
                    print("case 0");
                    UpdateResolution(direction);
                    break;
                case 1:
                    print("case 1");
                    UpdateTextureQuality(direction);
                    break;
                case 2:
                    print("case 2");
                    UpdateAntialiasing(direction);
                    break;
                case 3:
                    print("case 3");
                    UpdateVSync(direction);
                    break;
                default:
                    break;
            }

            yield return cycleSpeed;

            cyclingSetting = false;
            print("end of cycle");
        }
    }

    public void OnFullscreenToggle()
    {
        Screen.fullScreen = gameSettings.fullscreen = fullscreenToggle.isOn;
        videoApplyButton.interactable = true;
    }

    public void UpdateResolution(int operand)
    {
#if !UNITY_EDITOR
        currentRes = ((currentRes + resolutionOptions.Count) + operand) % resolutionOptions.Count;
        resolutionText.text = resolutionOptions[currentRes];
        Screen.SetResolution(resolutions[currentRes].width, resolutions[currentRes].height, Screen.fullScreen);
        gameSettings.resolutionIndex = currentRes;
        videoApplyButton.interactable = true;
#endif
    }

    public void UpdateTextureQuality(int operand)
    {
        currentTex = ((currentTex + textureQualityOptions.Count) + operand) % textureQualityOptions.Count;
        textureQualityText.text = textureQualityOptions[currentTex];
        QualitySettings.masterTextureLimit = gameSettings.textureQuality = currentTex;
        videoApplyButton.interactable = true;
    }

    public void UpdateAntialiasing(int operand)
    {
        currentAnt = ((currentAnt + antialiasingOptions.Count) + operand) % antialiasingOptions.Count;
        QualitySettings.antiAliasing = (int)Mathf.Pow(2, currentAnt);
        antialiasingText.text = antialiasingOptions[currentAnt];
        gameSettings.antialiasing = currentAnt;
        videoApplyButton.interactable = true;
    }

    public void UpdateVSync(int operand)
    {
        currentVsy = ((currentVsy + vSyncOptions.Count) + operand) % vSyncOptions.Count;
        vSyncText.text = vSyncOptions[currentVsy];
        QualitySettings.vSyncCount = gameSettings.vSync = currentVsy;
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
        try
        {
            gameSettings = JsonUtility.FromJson<GameSettings>(File.ReadAllText(Application.persistentDataPath + "/gamesettings.json"));
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }

        if (gameSettings == null) return;

        mouseSensitivitySlider.value = gameSettings.mouseSensitivity;
        controllerSensitivitySlider.value = gameSettings.controllerSensitivity;
        SetDefaultViewToggle();

        fullscreenToggle.isOn = gameSettings.fullscreen;

#if !UNITY_EDITOR
        currentRes = gameSettings.resolutionIndex;
        resolutionText.text = resolutionOptions[currentRes];
#endif

        currentTex = gameSettings.textureQuality;
        textureQualityText.text = textureQualityOptions[currentTex];

        currentAnt = gameSettings.antialiasing;
        antialiasingText.text = antialiasingOptions[currentAnt];

        currentVsy = gameSettings.vSync;
        vSyncText.text = vSyncOptions[currentVsy];

        masterVolumeSlider.value = gameSettings.masterVolume;

        Screen.fullScreen = gameSettings.fullscreen;
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

/*
 * Text Quality Options:
 * -Full Res
 * -Half Res
 * -Quarter Res
 * -Eighth Res
 * 
 * Antialiasing Options:
 * -Disabled
 * -2x
 * -4x
 * -8x
 * 
 * VSync Options:
 * -Disabled
 * -Every V Blank
 * -Every Second V Blank
 */