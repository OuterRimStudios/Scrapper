using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SelectedAbilityBox : MonoBehaviour {

	public Ability ability;
	public Button boxButton;
	public Image abilityIcon;
	public TextMeshProUGUI abilityName;

	public void InitializeSelected(Ability _ability)
	{
		ability = _ability;
		abilityIcon.sprite = _ability.abilityIcon;
		abilityName.text = _ability.abilityName;
	}
}
