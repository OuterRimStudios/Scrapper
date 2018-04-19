using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AbilityCard : MonoBehaviour {

	public Ability cardAbility;
	public Button cardButton;
	public Image abilityIcon;
	public TextMeshProUGUI abilityDescription;

	public void InitializeCard(Ability _cardAbility)
	{
		cardAbility = _cardAbility;
		abilityIcon.sprite = cardAbility.abilityIcon;
		abilityDescription.text = cardAbility.abilityDescription;
	}
}