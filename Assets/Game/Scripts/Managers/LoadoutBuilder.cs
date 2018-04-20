using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadoutBuilder : MonoBehaviour {

	public AbilityCard abilityCard;
	public SelectedAbilityBox selectedAbilityBox;

	[Space, Header("Display Areas")]
	public GameObject activeDisplayArea;
	public RectTransform selectedAbilitiesPanel;
	[Space]
	public RectTransform meleeAbilityPanel;
	public RectTransform mobilityAbilityPanel;
	public RectTransform moduleAbilityPanel;
	public RectTransform projectileAbilityPanel;
	public RectTransform spawnerAbilityPanel;
	public RectTransform sustainedAbilityPanel;
	public RectTransform trapAbilityPanel;
	public RectTransform turretAbilityPanel;

	[Space, Header("Ability Lists")]
	public List<Ability> meleeAbilities;
	public List<Ability> mobilityAbilities;
	public List<Ability> moduleAbilities;
	public List<Ability> projectileAbilities;
	public List<Ability> spawnersAbilities;
	public List<Ability> sustainedAbilities;
	public List<Ability> trapsAbilities;
	public List<Ability> turretsAbilities;
	public List<Ability> AllAbilities {get; protected set;}

	List<Ability> selectedAbilities = new List<Ability>();

	void Start()
	{
		AllAbilities = new List<Ability>();
		//InitializeCards(meleeAbilityPanel, meleeAbilities);
		InitializeCards(mobilityAbilityPanel, mobilityAbilities);
		InitializeCards(moduleAbilityPanel, moduleAbilities);
		InitializeCards(projectileAbilityPanel, projectileAbilities);
		//InitializeCards(spawnerAbilityPanel, spawnersAbilities);
		InitializeCards(sustainedAbilityPanel, sustainedAbilities);
		InitializeCards(trapAbilityPanel, trapsAbilities);
		InitializeCards(turretAbilityPanel, turretsAbilities);

		LoadoutPresetManager.instance.LoadPresets();
	}

	void InitializeCards(RectTransform panel, List<Ability> abilities)
	{
		foreach(Ability _ability in abilities)
		{
			if(!AllAbilities.Contains(_ability))
				AllAbilities.Add(_ability);

			AbilityCard newCard = Instantiate(abilityCard, panel);
			newCard.InitializeCard(_ability);
			newCard.cardButton.onClick.AddListener(delegate{AddToSelectedAbilities(_ability);});
		}
	}

	public void SwitchActiveDisplayArea(GameObject newActiveDisplay)
	{
		activeDisplayArea.SetActive(false);
		activeDisplayArea = newActiveDisplay;
		activeDisplayArea.SetActive(true);
	}

	public void AddToSelectedAbilities(Ability _ability)
	{
		if(selectedAbilitiesPanel.childCount < 20)
		{
			if(!selectedAbilities.Contains(_ability))
			{
				selectedAbilities.Add(_ability);
				SelectedAbilityBox newBox = Instantiate(selectedAbilityBox, selectedAbilitiesPanel);
				newBox.InitializeSelected(_ability);
				newBox.boxButton.onClick.AddListener(delegate{RemoveFromSelectedAbilities(newBox.gameObject, _ability);});
			}
		}
	}

	public void RemoveFromSelectedAbilities(GameObject button, Ability _ability)
	{
		selectedAbilities.Remove(_ability);
		Destroy(button);
	}

	public List<Ability> GetSelectedAbilities()
	{
		return selectedAbilities;
	}
}