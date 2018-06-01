using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    public delegate void AbilitiesUpdated();
	public static event AbilitiesUpdated OnAbilitiesUpdated;

	public LoadoutPreset currentLoadout;

	[Space, Header("Ability Loadout")]
    public AbilityDisplayArea abilityDisplayArea;
    [Tooltip("Ability Icons in the Player HUD")]
    public List<Image> abilityIcons;
    public List<TextMeshProUGUI> abilityCharges;
    public List<Image> abilityCooldownProgress;
    public List<Queue<float>> cooldownQueues = new List<Queue<float>>();

	public List<Ability> allAbilities = new List<Ability>();

    public Ability currentAbility;
    public int CurrentAbilityIndex { get; protected set; }
    public List<Ability> equippedAbilities;

	float remainingTime;

    void Start()
    {
        if(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == 0)
        {
            for(int i = 0; i < abilityDisplayArea.abilitySlots.Count; i++)
            {
                abilityDisplayArea.abilitySlots[i].GetComponent<Button>().onClick.AddListener(delegate { OnAbilitiesUpdated(); });
            }
        }
        else
            Initialize();
    }

    void OnEnable()
    {
        PlayerHealth.OnPlayerDied += ResetCooldowns;
    }

    void OnDisable()
    {
        PlayerHealth.OnPlayerDied -= ResetCooldowns;
    }

    void Update()
	{
		CheckCooldowns();
	}

	public void Initialize()
	{
        ResetCooldowns();

        for(int i = 0; i < equippedAbilities.Count; i++)
        {
            abilityIcons[i].sprite = equippedAbilities[i].abilityIcon;
            if(equippedAbilities[i].abilityCharges > 1)
            {
                abilityCharges[i].enabled = true;
                abilityCharges[i].text = equippedAbilities[i].abilityCharges.ToString();
                equippedAbilities[i].OnCooldownFinished += UpdateAbiltyCharges;
            }
            else
                abilityCharges[i].enabled = false;
        }

        InitializeAbilityDisplay();
    }

	void InitializeAbilityDisplay()
	{
		if(abilityDisplayArea)
			abilityDisplayArea.Initialize(equippedAbilities);

        UpdateAbilities(-1);
	}

    public void EquipAbilityItem(int abilitySlotIndex, AbilityItem abilityItem)
    {
        foreach(Ability ability in allAbilities)
        {
            if(abilityItem.ItemName == ability.abilityName)
            {
                ChangeAbility(abilitySlotIndex, ability);
            }
        }
    }

    public void ChangeAbility(int abilitySlotIndex, Ability newAbility)
    {
        if (abilitySlotIndex != -1)
        {
            equippedAbilities[abilitySlotIndex].OnCooldownFinished -= UpdateAbiltyCharges; //Move this to unsubscribe before changing abilities
            equippedAbilities[abilitySlotIndex] = newAbility;
       
            if (abilityCharges.Count > 0)
            {
                abilityCharges[abilitySlotIndex].text = equippedAbilities[abilitySlotIndex].charges.ToString();

                if (equippedAbilities[abilitySlotIndex].abilityCharges <= 1)
                    abilityCharges[abilitySlotIndex].enabled = false;
                else
                    abilityCharges[abilitySlotIndex].enabled = true;
            }
        }

        if (abilityIcons.Count > 0)
        {
            for (int i = 0; i < equippedAbilities.Count; i++)
            {
                equippedAbilities[i].OnCooldownFinished += UpdateAbiltyCharges; //move this to subscribe after changing abilities
                abilityIcons[i].sprite = equippedAbilities[i].abilityIcon;

                if (equippedAbilities[i].abilityCharges > 1)
                {
                    abilityCharges[i].text = equippedAbilities[i].abilityCharges.ToString();
                    abilityCharges[i].enabled = true;
                }
            }
        }
    }

	public void UpdateAbilities(int abilitySlotIndex)
	{
		if(abilitySlotIndex != -1)
        {
            equippedAbilities[abilitySlotIndex].OnCooldownFinished -= UpdateAbiltyCharges; //Move this to unsubscribe before changing abilities
            equippedAbilities[abilitySlotIndex] = abilityDisplayArea.currentActiveAbilitySlot.abilityInSlot;

            if(abilityCharges.Count > 0)
            {
                abilityCharges[abilitySlotIndex].text = equippedAbilities[abilitySlotIndex].charges.ToString();

                if (equippedAbilities[abilitySlotIndex].abilityCharges <= 1)
                    abilityCharges[abilitySlotIndex].enabled = false;
                else
                    abilityCharges[abilitySlotIndex].enabled = true;
            }
        }

        if(abilityIcons.Count > 0)
        {
            for(int i = 0; i < equippedAbilities.Count; i++)
            {
                equippedAbilities[i].OnCooldownFinished += UpdateAbiltyCharges; //move this to subscribe after changing abilities
                abilityIcons[i].sprite = equippedAbilities[i].abilityIcon;

                if(equippedAbilities[i].abilityCharges > 1)
                {
                    abilityCharges[i].text = equippedAbilities[i].abilityCharges.ToString();
                    abilityCharges[i].enabled = true;
                }
            }
        }
	}

    public void SetCurrentAbility(Ability newAbility)
    {
        if(equippedAbilities.Contains(newAbility))
        {
            currentAbility.DeactivateAbility();
            currentAbility = newAbility;
            CurrentAbilityIndex = equippedAbilities.IndexOf(newAbility);
        }
    }

    public void SetCurrentAbility(int abilityIndex)
    {
        if(abilityIndex < equippedAbilities.Count && abilityIndex > -1)
        {
            currentAbility.DeactivateAbility();
            currentAbility = equippedAbilities[abilityIndex];
            CurrentAbilityIndex = abilityIndex;
        }
    }

	void UpdateAbiltyCharges(Ability ability)
    {
        if(equippedAbilities.Contains(ability))
        {
            int abilityIndex = equippedAbilities.IndexOf(ability);
            abilityCharges[abilityIndex].text = ability.charges.ToString();
        }
    }

	void CheckCooldowns()
    {
        if(cooldownQueues.Count <= 0) return;

        for(int j = 0; j < equippedAbilities.Count; j++)
        {
            Cooldown(j);
        }

        for(int i = 0; i < cooldownQueues.Count; i++)
        {
            if(cooldownQueues[i].Count > 0)
                remainingTime = ((cooldownQueues[i].Peek() + equippedAbilities[i].abilityCooldown) - Time.time) / equippedAbilities[i].abilityCooldown;

            if(remainingTime < .01f && cooldownQueues[i].Count > 0)
                cooldownQueues[i].Dequeue();
        }
    }

    void Cooldown(int abilityIndex)
    {
        if(cooldownQueues.Count <= 0) return;
        if(cooldownQueues[abilityIndex].Count > 0)
        {
            float _remainingTime = ((cooldownQueues[abilityIndex].Peek() + equippedAbilities[abilityIndex].abilityCooldown) - Time.time) / equippedAbilities[abilityIndex].abilityCooldown;
            abilityCooldownProgress[abilityIndex].fillAmount = _remainingTime;

            if(_remainingTime < .01f)
                abilityCooldownProgress[abilityIndex].fillAmount = 0f;
        }
    }

    void ResetCooldowns()
    {
        cooldownQueues.Clear();

        for(int i = 0; i < 5; i++)
        {
            cooldownQueues.Add(new Queue<float>());
            abilityCooldownProgress[i].fillAmount = 0;
            equippedAbilities[i].ResetCooldown();
            abilityCharges[i].text = equippedAbilities[i].abilityCharges.ToString();
        }
    }
}