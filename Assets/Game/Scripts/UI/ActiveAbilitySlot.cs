﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActiveAbilitySlot : MonoBehaviour
{
    public AbilityDisplayArea abilityDisplayArea;
    public Text abilityDescriptionTextBox;

    //[HideInInspector]
    public Ability abilityInSlot;
    Image abilityIcon;

    [HideInInspector]
    public bool abilityActive;

    public enum SlotType
    {
        Active,
        Option
    }

    public SlotType slotType;

    private void Awake()
    {
        abilityIcon = transform.GetChild(0).GetChild(0).GetComponent<Image>();
        GetComponent<Button>().onClick.AddListener(() => abilityDisplayArea.SelectAbility(this));
    }

    public void ClearSlot()
    {
        abilityInSlot = null;
        if(abilityIcon)
            abilityIcon.sprite = null;
        abilityActive = false;
    }

    public void SetAbilitySlot(Ability newAbility)
    {
        if(!abilityIcon)
            abilityIcon = transform.GetChild(0).GetChild(0).GetComponent<Image>();

        abilityInSlot = newAbility;

        if(abilityInSlot == null) return;
        abilityIcon.sprite = abilityInSlot.abilityIcon;
    }

    public void AbilityActive(bool activeState)
    {
        abilityActive = activeState;
    }

    public void ViewDescription()
    {
        if(abilityInSlot == null) return;

        abilityDescriptionTextBox.text = abilityInSlot.abilityDescription;
    }

    public void StopViewingDescription()
    {
        if(abilityInSlot == null) return;
        abilityDescriptionTextBox.text = "";
    }
}