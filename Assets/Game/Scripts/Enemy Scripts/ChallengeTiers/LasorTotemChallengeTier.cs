using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ChallengeTier/Lasor Totem")]
public class LasorTotemChallengeTier : ChallengeTier {

	public int damage;
	public float damageFrequency;
	public float beamSize;

	public override void InitializeAbilityStats(Ability _ability)
    {
        SustainedTotemAI ability = _ability as SustainedTotemAI;
        for (int i = 0; i < ability.beams.Count; i++)
        {
            Sustained sustained = ability.beams[i];
            sustained.damage = damage;
            sustained.effectFrequency = damageFrequency;
            sustained.hitRadius = beamSize;
        }
    }
}