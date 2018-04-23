using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityManager : MonoBehaviour {

	#region Instance
    //Version of instance taken from "http://wiki.unity3d.com/index.php/AManagerClass"
    private static AbilityManager s_Instance = null;
    public static AbilityManager instance
    {
        get
        {
            if(s_Instance == null)
            {
                // This is where the magic happens.
                //  FindObjectOfType(...) returns the first AbilityManager object in the scene.
                s_Instance = FindObjectOfType(typeof(AbilityManager)) as AbilityManager;
            }

            // If it is still null, create a new instance
            if(s_Instance == null)
            {
                GameObject obj = new GameObject("AbilityManager");
                s_Instance = obj.AddComponent(typeof(AbilityManager)) as AbilityManager;
                Debug.Log("Could not locate an AbilityManager object. AbilityManager was Generated Automaticly.");
            }

            return s_Instance;
        }
    }
    #endregion

	public LoadoutPreset currentLoadout;

	[Space, Header("Ability Loadout")]
    public AbilityDisplayArea abilityDisplayArea;
    public List<ActiveAbilitySlot> activeAbilitySlots;
    [Tooltip("Ability Icons in the Player HUD")]
    public List<Image> abilityIcons;
    public List<Text> abilityCharges;
    public List<Image> abilityCooldownProgress;
    public List<Queue<float>> cooldownQueues = new List<Queue<float>>();

	public List<Ability> allAbilities = new List<Ability>();

	float remainingTime;

	void OnEnable()
	{
		LoadoutPresetManager.OnPresetsLoaded += Initialize;
	}

	void OnDisable()
	{
		LoadoutPresetManager.OnPresetsLoaded += Initialize;
	}

    void Start()
    {
        LoadoutPresetManager.instance.LoadPresets();
    }

	void Update()
	{
		CheckCooldowns();
	}

	public void Initialize()
	{
        cooldownQueues.Clear();

		for(int i = 0; i < 5; i++)
            cooldownQueues.Add(new Queue<float>());
		
        if(LoadoutPresetManager.instance.CurrentPreset != null)
		    currentLoadout = LoadoutPresetManager.instance.CurrentPreset;
        else
            currentLoadout = LoadoutPresetManager.instance.AllPresets[0];

		InitializeAbilityDisplay();
	}

	void InitializeAbilityDisplay()
	{
		if(abilityDisplayArea && currentLoadout != null)
			abilityDisplayArea.Initialize(currentLoadout.ActiveAbilities, currentLoadout.LoadoutAbilities);

        UpdateAbilities(-1);
	}

	public void UpdateAbilities(int abilitySlotIndex)
	{
		if(abilitySlotIndex != -1)
        {
            currentLoadout.LoadoutAbilities[abilitySlotIndex].OnCooldownFinished -= UpdateAbiltyCharges; //Move this to unsubscribe before changing abilities
            if(abilityDisplayArea.abilityLoadoutOptions.Contains(currentLoadout.LoadoutAbilities[abilitySlotIndex]))
            {
                int slotIndex = abilityDisplayArea.abilityLoadoutOptions.IndexOf(currentLoadout.ActiveAbilities[abilitySlotIndex]);
                abilityDisplayArea.loadoutAbilitySlots[slotIndex].AbilityActive(false);
            }
            currentLoadout.ActiveAbilities[abilitySlotIndex] = abilityDisplayArea.currentActiveAbilitySlot.abilityInSlot;

            if(abilityCharges.Count > 0)
            {
                abilityCharges[abilitySlotIndex].text = currentLoadout.ActiveAbilities[abilitySlotIndex].charges.ToString();

                if (currentLoadout.ActiveAbilities[abilitySlotIndex].abilityCharges <= 1)
                    abilityCharges[abilitySlotIndex].enabled = false;
                else
                    abilityCharges[abilitySlotIndex].enabled = true;
            }
        }

        if(abilityIcons.Count > 0)
        {
            for(int i = 0; i < currentLoadout.ActiveAbilities.Count; i++)
            {
                currentLoadout.ActiveAbilities[i].OnCooldownFinished += UpdateAbiltyCharges; //move this to subscribe after changing abilities
                abilityIcons[i].sprite = currentLoadout.ActiveAbilities[i].abilityIcon;

                if(currentLoadout.ActiveAbilities[i].abilityCharges > 1)
                {
                    abilityCharges[i].text = currentLoadout.ActiveAbilities[i].abilityCharges.ToString();
                    abilityCharges[i].enabled = true;
                }
            }
        }
	}

	void UpdateAbiltyCharges(Ability ability)
    {
        if(currentLoadout.ActiveAbilities.Contains(ability))
        {
            int abilityIndex = currentLoadout.ActiveAbilities.IndexOf(ability);
            abilityCharges[abilityIndex].text = ability.charges.ToString();
        }
    }

	void CheckCooldowns()
    {
        if(cooldownQueues.Count <= 0) return;

        for(int j = 0; j < currentLoadout.ActiveAbilities.Count; j++)
        {
            Cooldown(j);
        }

        for(int i = 0; i < cooldownQueues.Count; i++)
        {
            if(cooldownQueues[i].Count > 0)
                remainingTime = ((cooldownQueues[i].Peek() + currentLoadout.ActiveAbilities[i].abilityCooldown) - Time.time) / currentLoadout.ActiveAbilities[i].abilityCooldown;

            if(remainingTime < .01f && cooldownQueues[i].Count > 0)
                cooldownQueues[i].Dequeue();
        }
    }

    void Cooldown(int abilityIndex)
    {
        if(cooldownQueues.Count <= 0) return;
        if(cooldownQueues[abilityIndex].Count > 0)
        {
            float _remainingTime = ((cooldownQueues[abilityIndex].Peek() + currentLoadout.ActiveAbilities[abilityIndex].abilityCooldown) - Time.time) / currentLoadout.ActiveAbilities[abilityIndex].abilityCooldown;
            abilityCooldownProgress[abilityIndex].fillAmount = _remainingTime;

            if(_remainingTime < .01f)
                abilityCooldownProgress[abilityIndex].fillAmount = 0f;
        }
    }
}