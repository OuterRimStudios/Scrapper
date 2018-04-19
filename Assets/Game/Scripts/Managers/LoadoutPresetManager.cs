using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

	public XmlDocument LoadoutPresetDoc {get; protected set;}
	public string LoadoutPresetPath {get; protected set;}

	public void LoadPresets()
	{
		LoadoutPresetPath = Path.Combine(Application.streamingAssetsPath, "LoadoutPresets.xml");
		LoadoutPresetDoc = new XmlDocument();
		LoadFromXml(LoadoutPresetDoc, LoadoutPresetPath);
	}

	void LoadFromXml(XmlDocument xmlDoc, string path)
	{
		xmlDoc.Load(path);

		foreach(XmlNode preset in xmlDoc["Presets"].ChildNodes)
		{
			string presetName = "";
			List<string> activeAbilities = new List<string>();
			List<string> loadoutAbilities = new List<string>();

			if(preset.Attributes.Count > 0)
				presetName = preset.Attributes["name"].Value;
			
			foreach(XmlNode activeAbility in preset["ActiveAbilities"].ChildNodes)
			{
				print(activeAbility.InnerText);
				activeAbilities.Add(activeAbility.InnerText);
			}

			foreach(XmlNode loadoutAbility in preset["LoadoutAbilities"].ChildNodes)
			{
				print(loadoutAbility.InnerText);
				loadoutAbilities.Add(loadoutAbility.InnerText);
			}

			LoadoutPreset newPreset = new LoadoutPreset(presetName, activeAbilities, loadoutAbilities);
		}
	}

	void WriteToXML(LoadoutPreset oldLoadout, LoadoutPreset newLoadout)
	{
		RemoveFromXML(oldLoadout);
		AddPresetToXML(newLoadout);
	}

	void AddPresetToXML(LoadoutPreset loadout)
	{
		XmlNode preset = LoadoutPresetDoc.CreateElement("Preset");
		LoadoutPresetDoc.DocumentElement.AppendChild(preset);

		XmlAttribute nameAttr = LoadoutPresetDoc.CreateAttribute("name");
		nameAttr.Value = loadout.presetName;
		preset.Attributes.Append(nameAttr);

		XmlNode activeAbilities = LoadoutPresetDoc.CreateElement("ActiveAbilities");
		LoadoutPresetDoc.DocumentElement.AppendChild(activeAbilities);

		for(int i = 0; i < loadout.activeAbilities.Count; i++)
		{
			XmlNode ability = LoadoutPresetDoc.CreateElement("Ability");
			ability.InnerText = loadout.activeAbilities[i];
			activeAbilities.AppendChild(ability);
		}

		XmlNode loadoutAbilities = LoadoutPresetDoc.CreateElement("LoadoutAbilities");
		LoadoutPresetDoc.DocumentElement.AppendChild(loadoutAbilities);

		for(int i = 0; i < loadout.loadoutAbilities.Count; i++)
		{
			XmlNode ability = LoadoutPresetDoc.CreateElement("Ability");
			ability.InnerText = loadout.loadoutAbilities[i];
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
						if(ability.InnerText == loadout.activeAbilities[i])
							resultNode = preset;
						else
							return null;
					}

					for(int i = 0; i < preset["LoadoutAbilities"].ChildNodes.Count; i++)
					{
						XmlNode ability = preset["LoadoutAbilities"].ChildNodes[i];
						if(ability.InnerText == loadout.loadoutAbilities[i])
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
	public List<string> activeAbilities = new List<string>();
	public List<string> loadoutAbilities = new List<string>();

	public LoadoutPreset(string _presetName, List<string> _activeAbilities, List<string> _loadoutAbilities)
	{
		presetName = _presetName;

		foreach(string _active in _activeAbilities)
			activeAbilities.Add(_active);

		foreach(string _ability in _loadoutAbilities)
			loadoutAbilities.Add(_ability);
	}
}