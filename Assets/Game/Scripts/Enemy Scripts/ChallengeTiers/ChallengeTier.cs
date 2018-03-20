using UnityEngine;

public class ChallengeTier : ScriptableObject
{
    public int health;
    public int updateAtWave;

    [Space, Header("Optional Variables")]
    public int spawnCount;
    public float followSpeed;

    public virtual void InitializeAbilityStats(Ability ability) { }
}
