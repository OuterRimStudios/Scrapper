﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;

public class AIReferenceManager : ReferenceManager
{
    public AI ai;
	public List<ChallengeTier> easyChallengeTiers;
	public List<ChallengeTier> mediumChallengeTiers;
	public List<ChallengeTier> hardChallengeTiers;

	List<ChallengeTier> challengeTiers = new List<ChallengeTier> ();
	[HideInInspector] public ChallengeTier currentChallengeTier;
	int difficultyLevel;

	public Ability[] aiAbilities;

	protected override void Awake ()
	{
		base.Awake ();
        ai = GetComponent<AI>();
		statusEffects = GetComponent<StatusEffects> ();

		difficultyLevel = GameManager.currentDifficulty;// Until we can create a difficulty selection menu

		switch (difficultyLevel) {
		case 0:
			challengeTiers = easyChallengeTiers;
			break;

		case 1:
			challengeTiers = mediumChallengeTiers;
			break;

		case 2:
			challengeTiers = hardChallengeTiers;
			break;
		}

        UpdateChallengeTier(0);
    }

    public Ability GetAbility(string abilityName)
    {
        foreach (Ability ability in aiAbilities)
            if (ability.abilityName == abilityName)
                return ability;

        return null;
    }

    private void OnEnable()
    {
        UpdateChallengeTier(0);
    }

    public void UpdateChallengeTier(int waveCount)
    {
        for (int i = 0; i < challengeTiers.Count; i++)
        {
            if (challengeTiers[i].updateAtWave <= waveCount)
            {
                currentChallengeTier = challengeTiers[i];
            }
        }

        foreach(Ability ability in aiAbilities)
            currentChallengeTier.InitializeAbilityStats(ability);
    }
}

[System.Serializable]
public class SharedAIReferenceManager : SharedVariable<AIReferenceManager>
{
    public static implicit operator SharedAIReferenceManager(AIReferenceManager value) { return new SharedAIReferenceManager { Value = value }; }
}