using UnityEngine;

public class ChallengeTier : ScriptableObject
{
    public int health;
    public int updateAtWave;

    public virtual void InitializeAbilityStats(Ability ability) { }
}
