using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActiveAbilitySlot : MonoBehaviour
{
    public AbilityDisplayArea abilityDisplayArea;
    public Text abilityDescriptionTextBox;

    [HideInInspector]
    public Ability abilityInSlot;
    Image abilityIcon;

    [HideInInspector]
    public bool abilityActive;

    private void Awake()
    {
        abilityIcon = transform.GetChild(0).GetChild(0).GetComponent<Image>();
        GetComponent<Button>().onClick.AddListener(() => abilityDisplayArea.SelectAbility(this));
    }

    public void SetAbilitySlot(Ability newAbility)
    {
        if (!abilityIcon)
            abilityIcon = transform.GetChild(0).GetChild(0).GetComponent<Image>();

        abilityInSlot = newAbility;
        abilityIcon.sprite = abilityInSlot.abilityIcon;

       // print("Ability Icon: " + abilityIcon + "  Ability In Slot Icon: " + abilityInSlot.abilityIcon);
    }

    public void AbilityActive(bool activeState)
    {
        abilityActive = activeState;
    }

    public void ViewDescription()
    {
        if (!abilityDescriptionTextBox)
            abilityDescriptionTextBox = GameObject.FindGameObjectWithTag("AbilityDescriptionBox").GetComponent<Text>();

        abilityDescriptionTextBox.text = abilityInSlot.abilityDescription;
    }

    public void StopViewingDescription()
    {
        abilityDescriptionTextBox.text = "";
    }
}
