using UnityEngine;

public class ChallengeTier : ScriptableObject
{
    public int health;
    public int updateAtWave;

    [Space, Header("Optional Variables")]
    public int spawnCount;
    public float rotateSpeed;

    public virtual void InitializeAbilityStats(Ability ability) { }
}
