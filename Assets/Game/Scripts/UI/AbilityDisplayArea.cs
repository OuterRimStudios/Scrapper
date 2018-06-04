using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AbilityDisplayArea : MonoBehaviour
{
    #region Instance
    //Version of instance taken from "http://wiki.unity3d.com/index.php/AManagerClass"
    private static AbilityDisplayArea s_Instance = null;
    public static AbilityDisplayArea instance
    {
        get
        {
            if(s_Instance == null)
            {
                // This is where the magic happens.
                //  FindObjectOfType(...) returns the first AbilityDisplayArea object in the scene.
                s_Instance = FindObjectOfType(typeof(AbilityDisplayArea)) as AbilityDisplayArea;
            }

            // If it is still null, create a new instance
            if(s_Instance == null)
            {
                GameObject obj = new GameObject("AbilityDisplayArea");
                s_Instance = obj.AddComponent(typeof(AbilityDisplayArea)) as AbilityDisplayArea;
                Debug.Log("Could not locate an AbilityDisplayArea object. AbilityDisplayArea was Generated Automaticly.");
            }

            return s_Instance;
        }
    }
    #endregion

    public List<AbilitySlot> abilitySlots;
    public AbilitySlot newItemSlot;

    [HideInInspector]
    public AbilitySlot currentActiveAbilitySlot;

    bool lookingForAbility;

    public void Initialize(List<Ability> actvieAbilities)
    {
        s_Instance = this;
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

    public void SetNewItemSlot(Ability _ability)
    {
        newItemSlot.SetAbilitySlot(_ability);
    }
}