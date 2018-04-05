﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AIReferenceManager : ReferenceManager
{
	[HideInInspector] public AI ai;
	[HideInInspector] public StateController stateController;

	public Encounter encounter;
	public List<ChallengeTier> easyChallengeTiers;
	public List<ChallengeTier> mediumChallengeTiers;
	public List<ChallengeTier> hardChallengeTiers;

	List<ChallengeTier> challengeTiers = new List<ChallengeTier> ();
	[HideInInspector] public ChallengeTier currentChallengeTier;
	int difficultyLevel;

	public Ability aiAbility;

	SpawnManager spawnManager;

	protected override void Awake ()
	{
		base.Awake ();

		ai = GetComponent<AI> ();
		stateController = GetComponent<StateController> ();
		statusEffects = GetComponent<StatusEffects> ();

		difficultyLevel = SpawnManager.currentDifficulty;// Until we can create a difficulty selection menu

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

		spawnManager = GameObject.Find ("GameManager").GetComponent<SpawnManager> ();
		UpdateChallengeTier (spawnManager.currentWave);
	}

	public void UpdateChallengeTier (int waveCount)
	{
		for (int i = 0; i < challengeTiers.Count; i++) {
			if (challengeTiers [i].updateAtWave <= waveCount) {
				currentChallengeTier = challengeTiers [i];
			}
		}
		currentChallengeTier.InitializeAbilityStats (aiAbility);
	}
}