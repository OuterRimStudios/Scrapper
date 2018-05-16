using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    #region Instance
    //Version of instance taken from "http://wiki.unity3d.com/index.php/AManagerClass"
    private static MenuManager s_Instance = null;
    public static MenuManager instance
    {
        get
        {
            if(s_Instance == null)
            {
                // This is where the magic happens.
                //  FindObjectOfType(...) returns the first MenuManager object in the scene.
                s_Instance = FindObjectOfType(typeof(MenuManager)) as MenuManager;
            }

            // If it is still null, create a new instance
            if(s_Instance == null)
            {
                GameObject obj = new GameObject("MenuManager");
                s_Instance = obj.AddComponent(typeof(MenuManager)) as MenuManager;
                Debug.Log("Could not locate an MenuManager object. MenuManager was Generated Automaticly.");
            }

            return s_Instance;
        }
    }
    #endregion

    public ArenaSettings arenaSettings;
    public string ArenaSettingsPath { get; protected set; }
    public string MyDocumentsPath { get; protected set; }

    void Awake()
    {
        Time.timeScale = 1;
        MyDocumentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        arenaSettings = new ArenaSettings();

        ArenaSettingsPath = MyDocumentsPath + "/Scrapper/arenasettings.json";
        if(File.Exists(ArenaSettingsPath))
            LoadSettings();
        else
            SaveSettings();

        if(SceneManager.GetActiveScene().buildIndex == 0)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void LoadLevel(int levelNum)
    {
        LoadingScreenManager.LoadScene(levelNum);
    }

    public void SetSelectedPreset(string presetName)
    {
        arenaSettings.selectedPreset = presetName;
    }

    //0 = easy, 1 = medium, 2 = hard
    public void SetDifficulty(int difficulty)
    {
        arenaSettings.difficulty = difficulty;
    }

    public void SaveSettings()
    {
        string jsonData = JsonUtility.ToJson(arenaSettings, true);
        File.WriteAllText(ArenaSettingsPath, jsonData);
    }

    public void LoadSettings()
    {
        try
        {
            arenaSettings = JsonUtility.FromJson<ArenaSettings>(File.ReadAllText(ArenaSettingsPath));
        }
        catch(Exception e)
        {
            Debug.Log(e);
        }
    }

    public void Quit()
    {
        Application.Quit();
    }
}

public class ArenaSettings
{
    public string selectedPreset;
    public int difficulty;
}