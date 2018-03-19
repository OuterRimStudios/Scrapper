using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ChallengeTier/HealBot")]
public class HealBotChallengeTier : ChallengeTier
{
    public float speed;
    public float range;
    public int healAmount;
    public float healingFrequency;
}
