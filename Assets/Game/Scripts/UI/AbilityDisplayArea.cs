using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AbilityDisplayArea : MonoBehaviour
{
    public List<AbilitySlot> abilitySlots;

    [HideInInspector]
    public AbilitySlot currentActiveAbilitySlot;

    bool lookingForAbility;

    public void Initialize(List<Ability> actvieAbilities)
    {
        for(int k = 0; k < actvieAbilities.Count; k++)
            abilitySlots[k].SetAbilitySlot(actvieAbilities[k]);

        EventSystem.current.SetSelectedGameObject(abilitySlots[0].gameObject, null);
    }

    public void SelectAbility(AbilitySlot abilitySlot)
    {
        if(abilitySlot.abilityInSlot == null) return;

        if(lookingForAbility)
        {
            AbilitySlot oldSlot = currentActiveAbilitySlot;
            AbilitySlot newSlot = abilitySlot;

            int oldAbilitySlotIndex = abilitySlots.IndexOf(oldSlot);
            int newAbilitySlotIndex = abilitySlots.IndexOf(newSlot);

            Ability oldAbility = oldSlot.abilityInSlot;
            Ability newAbility = newSlot.abilityInSlot;

            AbilityManager.instance.SwapCooldownQueueus(oldAbilitySlotIndex, newAbilitySlotIndex);

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