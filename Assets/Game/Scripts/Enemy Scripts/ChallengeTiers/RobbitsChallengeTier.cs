using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "ChallengeTier/Rob Bits")]
public class RobbitsChallengeTier : ChallengeTier
{
    [Space, Header("Required Variables")]
    public int damage;
    public float attackFrequency;
    public float speed;

    public override void InitializeAbilityStats(Ability _ability)
    {
        Debug.Log("Upgrading Robbits");
        MeleeAbility ability = _ability as MeleeAbility;

        ability.damage = damage;
        ability.abilityCooldown = attackFrequency;
        ability.transform.root.GetComponent<NavMeshAgent>().speed = speed;
    }
}
