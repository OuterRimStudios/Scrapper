﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityDisplayArea : MonoBehaviour
{
    public List<Ability> abilityLoadoutOptions;                     //20 abilities in the loadout
    public List<ActiveAbilitySlot> activeAbilitySlots;              //5 active ability slots
    public List<ActiveAbilitySlot> loadoutAbilitySlots;             //Slots for the total loadout of 20 abilities

    [HideInInspector]
    public ActiveAbilitySlot currentActiveAbilitySlot;

    bool lookingForAbility;

    public void Initialize(List<Ability> actvieAbilities, List<Ability> loadoutAbilities)
    {
        abilityLoadoutOptions = new List<Ability>();

        for(int i = 0; i < loadoutAbilitySlots.Count; i++)
        {
            loadoutAbilitySlots[i].ClearSlot();
        }

        for(int j = 0; j < loadoutAbilities.Count; j++)
        {
            loadoutAbilitySlots[j].SetAbilitySlot(loadoutAbilities[j]);
            abilityLoadoutOptions.Add(loadoutAbilities[j]);
        }

        for(int k = 0; k < actvieAbilities.Count; k++)
        {
            activeAbilitySlots[k].SetAbilitySlot(actvieAbilities[k]);
            int index = abilityLoadoutOptions.IndexOf(actvieAbilities[k]);
            ActiveAbilitySlot abilitySlot = loadoutAbilitySlots[index];
            abilitySlot.AbilityActive(true);
        }
    }

    public void SelectAbility(ActiveAbilitySlot abilitySlot)
    {
        if(abilitySlot.abilityInSlot == null) return;

        if(abilitySlot.slotType == ActiveAbilitySlot.SlotType.Active)
        {
            lookingForAbility = true;
            currentActiveAbilitySlot = abilitySlot;

        }
        else if(abilitySlot.slotType == ActiveAbilitySlot.SlotType.Option && !abilitySlot.abilityActive && lookingForAbility)
        {
            currentActiveAbilitySlot.SetAbilitySlot(abilitySlot.abilityInSlot);
            currentActiveAbilitySlot.AbilityActive(false);
            abilitySlot.AbilityActive(true);
            int abilitySlotIndex = activeAbilitySlots.IndexOf(currentActiveAbilitySlot);
            AbilityManager.instance.UpdateAbilities(abilitySlotIndex);
            lookingForAbility = false;
        }
    }
}