using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityDisplayArea : MonoBehaviour
{
    public List<Ability> abilityLoadoutOptions;
    public List<ActiveAbilitySlot> activeAbilitySlots;
    public List<ActiveAbilitySlot> abilitySlots;
    public InputManager inputManager;

    [HideInInspector]
    public ActiveAbilitySlot currentActiveAbilitySlot;

    bool lookingForAbility;

    private void Start()
    {
        for(int i = 0; i < abilityLoadoutOptions.Count; i++)
        {
            abilitySlots[i].SetAbilitySlot(abilityLoadoutOptions[i]);
        }
    }

    public void SelectAbility(ActiveAbilitySlot abilitySlot)
    {
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
            inputManager.UpdateAbilities(abilitySlotIndex);
            lookingForAbility = false;
        }
    }
}