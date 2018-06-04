using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilitySlot : MonoBehaviour
{
    public AbilityDisplayArea abilityDisplayArea;
    public Text abilityDescriptionTextBox;

    //[HideInInspector]
    public Ability abilityInSlot;
    public Image abilityIcon;

    [HideInInspector]
    public bool abilityActive;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() => abilityDisplayArea.SelectAbility(this));

        if (abilityInSlot)
            SetAbilitySlot(abilityInSlot);
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