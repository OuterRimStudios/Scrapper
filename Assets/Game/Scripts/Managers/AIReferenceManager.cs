using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIReferenceManager : ReferenceManager
{
	[HideInInspector] public AI ai;
	[HideInInspector] public StateController stateController;

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

		ai = GetComponent<AI> ();
		stateController = GetComponent<StateController> ();
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