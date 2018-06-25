using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "ChallengeTier/HealBot")]
public class HealBotChallengeTier : ChallengeTier
{
    [Space, Header("Required Variables")]
    public float speed;
    public float range;
    public int healAmount;
    public float healingFrequency;

    public override void InitializeAbilityStats(Ability _ability)
    {
        SustainedHealingAI ability = _ability as SustainedHealingAI;

        for(int i = 0; i < ability.beams.Count; i++)
        {
            SustainedHealing sustainedHealing = ability.beams[i] as SustainedHealing;
            sustainedHealing.range = range;
            ability.refManager.stats.personalSpaceRange = range - 2;
            AIReferenceManager refManager = ability.refManager as AIReferenceManager;
            refManager.ai.agent.stoppingDistance = range - 2;
            sustainedHealing.healAmount = healAmount;
            sustainedHealing.effectFrequency = healingFrequency;
            sustainedHealing.transform.root.GetComponent<NavMeshAgent>().speed = speed;
        }
    }
}
