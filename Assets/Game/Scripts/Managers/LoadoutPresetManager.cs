using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadoutPresetManager : MonoBehaviour {
	#region Instance
    //Version of instance taken from "http://wiki.unity3d.com/index.php/AManagerClass"
    private static LoadoutPresetManager s_Instance = null;
    public static LoadoutPresetManager instance
    {
        get
        {
            if(s_Instance == null)
            {
                // This is where the magic happens.
                //  FindObjectOfType(...) returns the first LoadoutPresetManager object in the scene.
                s_Instance = FindObjectOfType(typeof(LoadoutPresetManager)) as LoadoutPresetManager;
            }

            // If it is still null, create a new instance
            if(s_Instance == null)
            {
                GameObject obj = new GameObject("LoadoutPresetManager");
                s_Instance = obj.AddComponent(typeof(LoadoutPresetManager)) as LoadoutPresetManager;
                Debug.Log("Could not locate an LoadoutPresetManager object. LoadoutPresetManager was Generated Automaticly.");
            }

            return s_Instance;
        }
    }
    #endregion

	public GameObject presetTabPrefab;
	public RectTransform loadoutTabsArea;
	public Button savePresetsButton;

	[Space, Header("Loadout Builder")]
	[Tooltip("Only necessary in scenes that have the LoadoutBuilder")]
	public LoadoutBuilder loadoutBuilder;
	public TextMeshProUGUI loadoutBuilderPresetText;

	[Space, Header("Loadout Menu")]
	public TextMeshProUGUI errorText;
	public TextMeshProUGUI presetNameText;
	public GameObject loadoutMenu;
	public AbilityDisplayArea abilityDisplayArea;

	public XmlDocument DefaultLoadoutPresetDoc {get; protected set;}
	public string DefaultLoadoutPresetPath {get; protected set;}
    public string MyDocumentsPath { get; protected set; }
    public XmlDocument LoadoutPresetDoc { get; protected set; }
    public string LoadoutPresetPath { get; protected set; }
    public LoadoutPreset CurrentPreset {get; protected set;}
	public List<LoadoutPreset> AllPresets {get; protected set;}

	List<GameObject> presetTabs = new List<GameObject>();
	int currentPresetIndex;

	void Awake()
	{
        MyDocumentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        
        if(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == 0)
		{
			LoadPresets();
			gameObject.SetActive(false);
		}
	}

	void OnEnable()
	{
		if(loadoutBuilder)
			LoadoutBuilder.OnLoadoutEdited += UpdatePreset;

		if(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == 0)
			AbilityManager.OnAbilitiesUpdated += UpdatePreset;

		LoadPresets();
		LoadPreset(GetSelectedPreset());
	}

	void OnDisable()
	{
		if(loadoutBuilder)
			LoadoutBuilder.OnLoadoutEdited -= UpdatePreset;

		if(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == 0)
			AbilityManager.OnAbilitiesUpdated -= UpdatePreset;

		ClearTabs();
	}

	void Start()
	{
		if(savePresetsButton)
			savePresetsButton.onClick.AddListener(delegate{ApplyChanges();});
	}

	public void LoadPresets()
	{
        if(!Directory.Exists(MyDocumentsPath + "/Scrapper/"))
        {
            Debug.LogWarning("Directory does not exist. Creating: " + MyDocumentsPath + "/Scrapper/");
            Directory.CreateDirectory(MyDocumentsPath + "/Scrapper/");
            if(!File.Exists(MyDocumentsPath + "/Scrapper/LoadoutPresets.xml"))
            {
                Debug.LogWarning("File does not exist. Creating: " + MyDocumentsPath + "/Scrapper/LoadoutPresets.xml");
                File.Create(MyDocumentsPath + "/Scrapper/LoadoutPresets.xml").Dispose();
                LoadoutPresetPath = MyDocumentsPath + "/Scrapper/LoadoutPresets.xml";
                LoadoutPresetDoc = new XmlDocument();

                DefaultLoadoutPresetPath = Path.Combine(Application.streamingAssetsPath, "LoadoutPresets.xml");
                DefaultLoadoutPresetDoc = new XmlDocument();
                LoadFromXml(DefaultLoadoutPresetDoc, DefaultLoadoutPresetPath);

                foreach(LoadoutPreset preset in AllPresets)
                    AddPresetToXML(preset);
            }
        }
		else
        {
            if(!File.Exists(MyDocumentsPath + "/Scrapper/LoadoutPresets.xml"))
            {
                Debug.LogWarning("File does not exist. Creating: " + MyDocumentsPath + "/Scrapper/LoadoutPresets.xml");
                File.Create(MyDocumentsPath + "/Scrapper/LoadoutPresets.xml").Dispose();
                LoadoutPresetPath = MyDocumentsPath + "/Scrapper/LoadoutPresets.xml";
                LoadoutPresetDoc = new XmlDocument();

                DefaultLoadoutPresetPath = Path.Combine(Application.streamingAssetsPath, "LoadoutPresets.xml");
                DefaultLoadoutPresetDoc = new XmlDocument();
                LoadFromXml(DefaultLoadoutPresetDoc, DefaultLoadoutPresetPath);

                foreach(LoadoutPreset preset in AllPresets)
                    AddPresetToXML(preset);
            }
            else
            {
                LoadoutPresetPath = MyDocumentsPath + "/Scrapper/LoadoutPresets.xml";
                LoadoutPresetDoc = new XmlDocument();
                LoadFromXml(LoadoutPresetDoc, LoadoutPresetPath);
            }
        }
	}

	void LoadFromXml(XmlDocument xmlDoc, string path)
	{
		xmlDoc.Load(path);

		AllPresets = new List<LoadoutPreset>();

		foreach(XmlNode preset in xmlDoc["Presets"].ChildNodes)
		{
			string presetName = "";
			List<string> activeAbilities = new List<string>();
			List<string> loadoutAbilities = new List<string>();

			if(preset.Attributes.Count > 0)
				presetName = preset.Attributes["name"].Value;
			
			foreach(XmlNode activeAbility in preset["ActiveAbilities"].ChildNodes)
			{
				activeAbilities.Add(activeAbility.InnerText);
			}

			foreach(XmlNode loadoutAbility in preset["LoadoutAbilities"].ChildNodes)
			{
				loadoutAbilities.Add(loadoutAbility.InnerText);
			}

			LoadoutPreset newPreset = new LoadoutPreset(presetName, activeAbilities, loadoutAbilities);
			AllPresets.Add(newPreset);
			CreateTab(newPreset);
		}
	}

	void CreateTab(LoadoutPreset _preset)
	{
		GameObject newTab = Instantiate(presetTabPrefab, loadoutTabsArea);
		presetTabs.Add(newTab);
		newTab.transform.SetAsFirstSibling();
		TextMeshProUGUI buttonText = newTab.GetComponentInChildren<TextMeshProUGUI>();
		if(buttonText)
			buttonText.text = _preset.presetName;

		Button button = newTab.GetComponent<Button>();
		if(button)
		{
			button.onClick.AddListener(delegate{LoadPreset(_preset);});
		}
	}

	void ClearTabs()
	{
		CurrentPreset = null;
		AllPresets.Clear();

		foreach(GameObject tab in presetTabs)
			Destroy(tab);
	}

	void ResetTabs()
	{
		ClearTabs();
		LoadPresets();
	}

	public void ToggleLoadoutMenu()
	{
		if(loadoutMenu.activeSelf)
		{
			loadoutMenu.SetActive(!loadoutMenu.activeSelf);
		}
		else
		{
			if(CurrentPreset.LoadoutAbilities.Count < 6)
			{
                errorText.text = "You need to select more abilities to open the loadout window.";
                errorText.enabled = true;
			}
			else
			{
				errorText.enabled = false;
				loadoutMenu.SetActive(!loadoutMenu.activeSelf);
			}
		}
	}

	public void LoadPreset(LoadoutPreset _preset)
	{
		CurrentPreset = _preset;

		if(loadoutBuilderPresetText)
		{
			loadoutBuilderPresetText.text = CurrentPreset.presetName;
			loadoutBuilderPresetText.enabled = true;
		}

		if(presetNameText)
			presetNameText.text = CurrentPreset.presetName;

		if(AllPresets.Contains(_preset))
			currentPresetIndex = AllPresets.IndexOf(_preset);

		if(loadoutBuilder)
		{
			loadoutBuilder.ClearSelectedAbilities();

			foreach(Ability _ability in _preset.LoadoutAbilities)
			{
				loadoutBuilder.AddToSelectedAbilities(_ability);
			}
		}

		if(_preset.LoadoutAbilities.Count > 5)
			AbilityManager.instance.Initialize();
	}

	LoadoutPreset GetSelectedPreset()
	{
		for(int i = 0; i < AllPresets.Count; i++)
		{
			if(AllPresets[i].presetName == MenuManager.instance.arenaSettings.selectedPreset)
			{
				return AllPresets[i];
			}
		}

		return null;
	}

	void UpdatePreset()
	{
		string presetName = CurrentPreset.presetName;
		List<string> activeAbilityNames = new List<string>();
		List<string> loadoutAbilityNames = new List<string>();

		if(loadoutBuilder)
		{
			foreach(Ability _ability in loadoutBuilder.SelectedAbilities)
				loadoutAbilityNames.Add(_ability.abilityName);

			foreach(ActiveAbilitySlot activeSlot in abilityDisplayArea.activeAbilitySlots)
			{
				if(!loadoutBuilder.SelectedAbilities.Contains(activeSlot.abilityInSlot))
				{
					for(int i = 0; i < loadoutBuilder.SelectedAbilities.Count; i++)
					{
						if(!activeAbilityNames.Contains(loadoutBuilder.SelectedAbilities[i].abilityName))
						{
							List<string> activeNames = new List<string>();
							for(int j = 0; j < abilityDisplayArea.activeAbilitySlots.Count; j++)
							{
								if(abilityDisplayArea.activeAbilitySlots[j].abilityInSlot != null)
									activeNames.Add(abilityDisplayArea.activeAbilitySlots[j].abilityInSlot.abilityName);
							}

							if(!activeNames.Contains(loadoutBuilder.SelectedAbilities[i].abilityName))
							{
								activeAbilityNames.Add(loadoutBuilder.SelectedAbilities[i].abilityName);
								break;
							}
						}
					}
				}
				else
					activeAbilityNames.Add(activeSlot.abilityInSlot.abilityName);
			}
		}

		LoadoutPreset updatedPreset = new LoadoutPreset(presetName, activeAbilityNames, loadoutAbilityNames);
		CurrentPreset = updatedPreset;

		if(CurrentPreset.LoadoutAbilities.Count > 5)
			AbilityManager.instance.Initialize();

		savePresetsButton.interactable = true;
	}

	public void ApplyChanges()
	{
        if(CurrentPreset.LoadoutAbilities.Count < 6)
        {
            errorText.text = "You need to select more abilities to save your preset.";
            errorText.enabled = true;
            return;
        }
        else
            errorText.enabled = false;

        savePresetsButton.interactable = false;
		string presetName = CurrentPreset.presetName;
		List<string> activeAbilityNames = new List<string>();
		List<string> loadoutAbilityNames = new List<string>();

		foreach(ActiveAbilitySlot activeSlot in abilityDisplayArea.activeAbilitySlots)
			activeAbilityNames.Add(activeSlot.abilityInSlot.name);

		foreach(Ability loadoutAbility in loadoutBuilder.SelectedAbilities)
			loadoutAbilityNames.Add(loadoutAbility.abilityName);

		LoadoutPreset newPreset = new LoadoutPreset(presetName, activeAbilityNames, loadoutAbilityNames);

		WriteToXML(AllPresets[currentPresetIndex], newPreset);
	}

	void WriteToXML(LoadoutPreset oldLoadout, LoadoutPreset newLoadout)
	{
        if(oldLoadout != null)
		    RemoveFromXML(oldLoadout);
		AddPresetToXML(newLoadout);
		ResetTabs();
	}

	void AddPresetToXML(LoadoutPreset loadout)
	{
        if(LoadoutPresetDoc.DocumentElement == null)
        {
            LoadoutPresetDoc = new XmlDocument();
            XmlDeclaration xmlDec = LoadoutPresetDoc.CreateXmlDeclaration("1.0", "UTF-8", null);
            XmlElement root = LoadoutPresetDoc.DocumentElement;
            LoadoutPresetDoc.InsertBefore(xmlDec, root);
            XmlElement presets = LoadoutPresetDoc.CreateElement("Presets");
            LoadoutPresetDoc.AppendChild(presets);
        }

		XmlNode preset = LoadoutPresetDoc.CreateElement("Preset");
        LoadoutPresetDoc.DocumentElement.AppendChild(preset);

		XmlAttribute nameAttr = LoadoutPresetDoc.CreateAttribute("name");
		nameAttr.Value = loadout.presetName;
		preset.Attributes.Prepend(nameAttr);

		XmlNode activeAbilities = LoadoutPresetDoc.CreateElement("ActiveAbilities");
		preset.AppendChild(activeAbilities);

		for(int i = 0; i < loadout.activeAbilityNames.Count; i++)
		{
			XmlNode ability = LoadoutPresetDoc.CreateElement("Ability");
			ability.InnerText = loadout.activeAbilityNames[i];
			activeAbilities.AppendChild(ability);
		}

		XmlNode loadoutAbilities = LoadoutPresetDoc.CreateElement("LoadoutAbilities");
		preset.AppendChild(loadoutAbilities);

		for(int i = 0; i < loadout.loadoutAbilityNames.Count; i++)
		{
			XmlNode ability = LoadoutPresetDoc.CreateElement("Ability");
			ability.InnerText = loadout.loadoutAbilityNames[i];
			loadoutAbilities.AppendChild(ability);
		}

        LoadoutPresetDoc.Save(LoadoutPresetPath);
	}

	void RemoveFromXML(LoadoutPreset loadout)
	{
		XmlNode nodeToRemove = GetXmlNode(loadout);

		if(nodeToRemove != null)
		{
			XmlNode prevNode = nodeToRemove.PreviousSibling;
			nodeToRemove.OwnerDocument.DocumentElement.RemoveChild(nodeToRemove);

			if (prevNode != null && (prevNode.NodeType == XmlNodeType.Whitespace || prevNode.NodeType == XmlNodeType.SignificantWhitespace))
                prevNode.OwnerDocument.DocumentElement.RemoveChild(prevNode);
		}

        LoadoutPresetDoc.Save(LoadoutPresetPath);
    }

	XmlNode GetXmlNode(LoadoutPreset loadout)
	{
		XmlNode resultNode = null;
		foreach(XmlNode preset in LoadoutPresetDoc["Presets"].ChildNodes)
		{
			if(preset.Attributes.Count > 0)
			{
				if(preset.Attributes["name"].Value == loadout.presetName)
				{
					for(int i = 0; i < preset["ActiveAbilities"].ChildNodes.Count; i++)
					{
						XmlNode ability = preset["ActiveAbilities"].ChildNodes[i];
						if(ability.InnerText == loadout.activeAbilityNames[i])
							resultNode = preset;
						else
							return null;
					}

					for(int i = 0; i < preset["LoadoutAbilities"].ChildNodes.Count; i++)
					{
						XmlNode ability = preset["LoadoutAbilities"].ChildNodes[i];
						if(ability.InnerText == loadout.loadoutAbilityNames[i])
							resultNode = preset;
						else
							return null;
					}
				}
			}
		}
		return resultNode;
	}
}

public class LoadoutPreset
{
	public string presetName;
	public List<string> activeAbilityNames = new List<string>();
	public List<string> loadoutAbilityNames = new List<string>();
	public List<Ability> ActiveAbilities {get; protected set;}
	public List<Ability> LoadoutAbilities {get; protected set;}

	public LoadoutPreset(string _presetName, List<string> _activeAbilities, List<string> _loadoutAbilities)
	{
		presetName = _presetName;
		ActiveAbilities = new List<Ability>();
		LoadoutAbilities = new List<Ability>();

		foreach(string _active in _activeAbilities)
		{
			activeAbilityNames.Add(_active);
			ActiveAbilities.Add(GetAbilityByName(_active));
		}

		foreach(string _ability in _loadoutAbilities)
		{
			loadoutAbilityNames.Add(_ability);
			LoadoutAbilities.Add(GetAbilityByName(_ability));
		}
	}

	public Ability GetAbilityByName(string abilityName)
	{
		Ability result = null;
		for(int i = 0; i < AbilityManager.instance.allAbilities.Count; i++)
		{
			if(AbilityManager.instance.allAbilities[i].abilityName == abilityName)
				result = AbilityManager.instance.allAbilities[i];
		}

		return result;
	}
}