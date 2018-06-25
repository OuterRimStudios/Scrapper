using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ChallengeTier/Pulsator")]
public class PulsatorChallengeTier : ChallengeTier
{
    [Space, Header("Required Variables")]
    public int damage;
    public float pulseFrequency;

    public override void InitializeAbilityStats(Ability ability)
    {
        PulseAbilityAI _ability = ability as PulseAbilityAI;
        _ability.initialDamage = damage;
        _ability.abilityCooldown = pulseFrequency;
    }
}
