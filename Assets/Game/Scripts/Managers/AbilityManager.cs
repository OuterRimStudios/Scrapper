﻿using System.Collections;
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
    [Space, Header("Radial Menu Abilities")]
    public List<Image> abilityIcons;
    public List<TextMeshProUGUI> abilityCharges;
    public List<Image> abilityCooldownProgress;
    public List<List<float>> cooldownQueues = new List<List<float>>();

    [Space, Header("Movement Ability")]
    public Ability movementAbility;
    public Image movementAbilityIcon;
    public TextMeshProUGUI movementAbilityCharges;
    public Image movementAbilityCooldownProgress;
    [HideInInspector]
    public List<float> movementAbilityCooldownQueue;

    [Space]
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

        movementAbilityIcon.sprite = movementAbility.abilityIcon;

        if (movementAbility.abilityCharges > 1)
        {
            movementAbilityCharges.enabled = true;
            movementAbilityCharges.text = movementAbility.abilityCharges.ToString();
            movementAbility.OnCooldownFinished += UpdateMovementAbilityCharges;
        }
        else
            movementAbilityCharges.enabled = false;

        InitializeAbilityDisplay();
    }

	void InitializeAbilityDisplay()
	{
		if(abilityDisplayArea)
			abilityDisplayArea.Initialize(equippedAbilities, movementAbility);

        UpdateAbilities(-1);
	}

    public void EquipAbilityItem(int abilitySlotIndex, AbilityItem abilityItem)
    {
        foreach(Ability ability in allAbilities)
        {
            if(abilityItem.ItemName == ability.abilityName)
            {
                if(abilityItem.ability.abilityType == Ability.AbilityType.Mobility)
                    ChangeMovementAbility(ability);
                else
                    ChangeAbility(abilitySlotIndex, ability);
            }
        }
    }

    public void ChangeAbility(int abilitySlotIndex, Ability newAbility)
    {
        if(!newAbility.gameObject.activeInHierarchy)                    //If this is false, then newAbility is probably referencing the prefab
            newAbility = GetAbilityByName(newAbility.abilityName);

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

    public void ChangeMovementAbility(Ability newAbility)
    {
        if (!newAbility.gameObject.activeInHierarchy)                    //If this is false, then newAbility is probably referencing the prefab
            newAbility = GetAbilityByName(newAbility.abilityName);

            movementAbility.OnCooldownFinished -= UpdateMovementAbilityCharges; //Move this to unsubscribe before changing abilities
            movementAbility = newAbility;

            if (movementAbilityCharges.isActiveAndEnabled)
            {
                movementAbilityCharges.text = movementAbility.charges.ToString();

                if (movementAbility.abilityCharges <= 1)
                    movementAbilityCharges.enabled = false;
                else
                    movementAbilityCharges.enabled = true;
            }
            
            movementAbility.OnCooldownFinished += UpdateMovementAbilityCharges; //move this to subscribe after changing abilities
            movementAbilityIcon.sprite = movementAbility.abilityIcon;

            if (movementAbility.abilityCharges > 1)
            {
                movementAbilityCharges.text = movementAbility.abilityCharges.ToString();
                movementAbilityCharges.enabled = true;
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

    public void UpdateMovementAbility()
    {
        movementAbility.OnCooldownFinished -= UpdateMovementAbilityCharges; //Move this to unsubscribe before changing abilities
        movementAbility = abilityDisplayArea.currentActiveMovementAbilitySlot.abilityInSlot;

        if (movementAbilityCharges.isActiveAndEnabled)
        {
            movementAbilityCharges.text = movementAbility.charges.ToString();

            if (movementAbility.abilityCharges <= 1)
                movementAbilityCharges.enabled = false;
            else
                movementAbilityCharges.enabled = true;
        }

        movementAbility.OnCooldownFinished += UpdateMovementAbilityCharges; //move this to subscribe after changing abilities
        movementAbilityIcon.sprite = movementAbility.abilityIcon;

        if (movementAbility.abilityCharges > 1)
        {
            movementAbilityCharges.text = movementAbility.abilityCharges.ToString();
            movementAbilityCharges.enabled = true;
        }
    }

    public void SwapCooldownQueueus(int indexOne, int indexTwo)
    {
        if (indexOne != -1 && indexTwo != -1)
        {
            List<float> tempQueue = new List<float>();
            foreach (float cooldown in cooldownQueues[indexTwo])
                tempQueue.Add(cooldown);

            cooldownQueues[indexTwo].Clear();

            foreach (float cooldown in cooldownQueues[indexOne])
                cooldownQueues[indexTwo].Add(cooldown);

            cooldownQueues[indexOne].Clear();

            foreach (float cooldown in tempQueue)
                cooldownQueues[indexOne].Add(cooldown);
        }
        else
            ResetCooldowns();
        //Removed if it becomes exploitable
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

    void UpdateMovementAbilityCharges(Ability ability)
    {
        if (movementAbility == ability)
        {
            movementAbilityCharges.text = ability.charges.ToString();
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
                remainingTime = ((cooldownQueues[i][0] + equippedAbilities[i].abilityCooldown) - Time.time) / equippedAbilities[i].abilityCooldown;

            if(remainingTime < .01f && cooldownQueues[i].Count > 0)
                cooldownQueues[i].RemoveAt(0);
        }
    }

    void CheckMovementCooldowns()
    {
        if (movementAbilityCooldownQueue.Count <= 0) return;

        MovementCooldown();

        for (int i = 0; i < movementAbilityCooldownQueue.Count; i++)
        {
            if (movementAbilityCooldownQueue.Count > 0)
                remainingTime = ((movementAbilityCooldownQueue[0] + movementAbility.abilityCooldown) - Time.time) / movementAbility.abilityCooldown;

            if (remainingTime < .01f && cooldownQueues[i].Count > 0)
                movementAbilityCooldownQueue.RemoveAt(0);
        }
    }

    void Cooldown(int abilityIndex)
    {
        if (cooldownQueues.Count <= 0)
            return;

        if(cooldownQueues[abilityIndex].Count > 0)
        {
            float _remainingTime = ((cooldownQueues[abilityIndex][0] + equippedAbilities[abilityIndex].abilityCooldown) - Time.time) / equippedAbilities[abilityIndex].abilityCooldown;
            abilityCooldownProgress[abilityIndex].fillAmount = _remainingTime;

            if(_remainingTime < .01f)
                abilityCooldownProgress[abilityIndex].fillAmount = 0f;
        }
        else
            abilityCooldownProgress[abilityIndex].fillAmount = 0f;
    }


    void MovementCooldown()
    {
        if (movementAbilityCooldownQueue.Count <= 0)
            return;

        if (movementAbilityCooldownQueue.Count > 0)
        {
            float _remainingTime = ((movementAbilityCooldownQueue[0] + movementAbility.abilityCooldown) - Time.time) / movementAbility.abilityCooldown;
            movementAbilityCooldownProgress.fillAmount = _remainingTime;

            if (_remainingTime < .01f)
                movementAbilityCooldownProgress.fillAmount = 0f;
        }
        else
            movementAbilityCooldownProgress.fillAmount = 0f;
    }


    void ResetCooldowns()
    {
        cooldownQueues.Clear();

        for(int i = 0; i < 5; i++)
        {
            cooldownQueues.Add(new List<float>());
            abilityCooldownProgress[i].fillAmount = 0;
            equippedAbilities[i].ResetCooldown();
            abilityCharges[i].text = equippedAbilities[i].abilityCharges.ToString();
        }

        movementAbilityCooldownQueue.Add(new float());
        movementAbilityCooldownProgress.fillAmount = 0;
        movementAbility.ResetCooldown();
        movementAbilityCharges.text = movementAbility.abilityCharges.ToString();
    }

    public Ability GetAbilityByName(string abilityName)
    {
        foreach(Ability ability in allAbilities)
        {
            if(abilityName == ability.abilityName)
            {
                return ability;
            }
        }

        Debug.LogError("No item found with the name: " + abilityName);
        return null;
    }
}