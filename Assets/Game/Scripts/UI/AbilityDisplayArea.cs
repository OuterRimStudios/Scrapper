using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityDisplayArea : MonoBehaviour
{
    public List<ActiveAbilitySlot> activeAbilitySlots;

    [HideInInspector]
    public ActiveAbilitySlot currentActiveAbilitySlot;

    bool lookingForAbility;

    public void Initialize(List<Ability> actvieAbilities)
    {
        for(int k = 0; k < actvieAbilities.Count; k++)
            activeAbilitySlots[k].SetAbilitySlot(actvieAbilities[k]);
    }

    public void SelectAbility(ActiveAbilitySlot abilitySlot)
    {
        if(abilitySlot.abilityInSlot == null) return;

        if(abilitySlot.slotType == ActiveAbilitySlot.SlotType.Active)
        {
            if(lookingForAbility)
            {
                ActiveAbilitySlot oldSlot = currentActiveAbilitySlot;
                ActiveAbilitySlot newSlot = abilitySlot;

                int oldAbilitySlotIndex = activeAbilitySlots.IndexOf(oldSlot);
                int newAbilitySlotIndex = activeAbilitySlots.IndexOf(newSlot);

                Ability oldAbility = oldSlot.abilityInSlot;
                Ability newAbility = newSlot.abilityInSlot;
                
                oldSlot.SetAbilitySlot(newAbility);
                newSlot.SetAbilitySlot(oldAbility);

                AbilityManager.instance.ChangeAbility(oldAbilitySlotIndex, newAbility);
                AbilityManager.instance.ChangeAbility(newAbilitySlotIndex, oldAbility);

                lookingForAbility = false;
            }
            else
            {
                lookingForAbility = true;
                currentActiveAbilitySlot = abilitySlot;
            }
        }
    }
}