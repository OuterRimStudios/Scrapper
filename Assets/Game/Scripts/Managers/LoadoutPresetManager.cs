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

	public delegate void PresetsLoaded();
	public static event PresetsLoaded OnPresetsLoaded;

	public GameObject presetTabPrefab;
	public RectTransform loadoutTabsArea;
	public Button savePresetsButton;

	[Space, Header("Loadout Builder")]
	[Tooltip("Only necessary in scenes that have the LoadoutBuilder")]
	public LoadoutBuilder loadoutBuilder;

	[Space, Header("Loadout Menu")]
	public GameObject loadoutMenu;
	public AbilityDisplayArea abilityDisplayArea;

	public XmlDocument LoadoutPresetDoc {get; protected set;}
	public string LoadoutPresetPath {get; protected set;}
	public LoadoutPreset CurrentPreset {get; protected set;}
	public List<LoadoutPreset> AllPresets {get; protected set;}

	List<GameObject> presetTabs = new List<GameObject>();
	int currentPresetIndex;

	void OnEnable()
	{
		if(loadoutBuilder)
			LoadoutBuilder.OnLoadoutEdited += UpdatePreset;
	}

	void OnDisable()
	{
		if(loadoutBuilder)
			LoadoutBuilder.OnLoadoutEdited -= UpdatePreset;
	}

	void Start()
	{
		if(savePresetsButton)
			savePresetsButton.onClick.AddListener(delegate{ApplyChanges();});

		LoadPresets();
		LoadPreset(AllPresets[0]);
	}

	public void LoadPresets()
	{
		LoadoutPresetPath = Path.Combine(Application.streamingAssetsPath, "LoadoutPresets.xml");
		LoadoutPresetDoc = new XmlDocument();
		LoadFromXml(LoadoutPresetDoc, LoadoutPresetPath);
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

		//OnPresetsLoaded();
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

	void ResetTabs()
	{
		CurrentPreset = null;
		AllPresets.Clear();

		foreach(GameObject tab in presetTabs)
			Destroy(tab);

		LoadPresets();
	}

	public void ToggleLoadoutMenu()
	{
		loadoutMenu.SetActive(!loadoutMenu.activeSelf);
	}

	public void LoadPreset(LoadoutPreset _preset)
	{
		CurrentPreset = _preset;

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

		AbilityManager.instance.Initialize();
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
		AbilityManager.instance.Initialize();
		savePresetsButton.interactable = true;
	}

	public void ApplyChanges()
	{
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

		//CurrentPreset = newPreset;
		//AllPresets[currentPresetIndex] = newPreset;
	}

	void WriteToXML(LoadoutPreset oldLoadout, LoadoutPreset newLoadout)
	{
		RemoveFromXML(oldLoadout);
		AddPresetToXML(newLoadout);
		ResetTabs();
	}

	void AddPresetToXML(LoadoutPreset loadout)
	{
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