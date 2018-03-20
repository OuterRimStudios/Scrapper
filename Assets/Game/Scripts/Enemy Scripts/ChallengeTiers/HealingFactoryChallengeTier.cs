using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ChallengeTier/HealingFactory")]
public class HealingFactoryChallengeTier : ChallengeTier
{
    [Space, Header("Required Variables")]
    [Tooltip("Spawn Frequency")]
    public float abilityCooldown;
    public int amountToSpawn;

    public override void InitializeAbilityStats(Ability _ability)
    {
        SpawnAIAbility ability = _ability as SpawnAIAbility;
        ability.abilityCooldown = abilityCooldown;
        ability.amountToSpawn = amountToSpawn;
    }
}
