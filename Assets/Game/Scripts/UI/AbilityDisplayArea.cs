using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityDisplayArea : MonoBehaviour
{
    public List<Ability> abilityLoadoutOptions;
    public List<ActiveAbilitySlot> abilitySlots;

    ActiveAbilitySlot currentActiveAbilitySlot;

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
        if(abilitySlot.abilityActive)
        {
            lookingForAbility = true;
            currentActiveAbilitySlot = abilitySlot;
        }
        else if(!abilitySlot.abilityActive && lookingForAbility)
        {
            currentActiveAbilitySlot.SetAbilitySlot(abilitySlot.abilityInSlot);
            currentActiveAbilitySlot.AbilityActive(false);
            abilitySlot.AbilityActive(true);
            lookingForAbility = false;
        }
    }
}
