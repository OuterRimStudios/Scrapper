using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiProjectileAbility : Ability
{
    public Projectile projectile;
    public int initialDamage;
    public bool alternateFire;

    List<Transform> availableSpawnPositions = new List<Transform>();

    protected override void Start()
    {
        base.Start();
        ResetSpawnpoints();    
    }

    public override void ActivateAbility()
    {
        base.ActivateAbility();

        if(!alternateFire)
        {
            for (int i = 0; i < refManager.SpawnPosition().Length; i++)
            {
                Projectile _projectile = Instantiate(projectile, refManager.SpawnPosition()[i].position, refManager.SpawnPosition()[i].rotation);
                _projectile.Initialize(initialDamage, refManager.enemyTag.ToString(), refManager.friendlyTag.ToString(), afterEffects);

                for (int j = 0; j < GetActiveModules().Count; j++)
                    _projectile.SetModule(GetActiveModules()[j]);
            }
        }
        else
        {
            Projectile _projectile = Instantiate(projectile, GetSpawnPosition().position, GetSpawnPosition().rotation);
            _projectile.Initialize(initialDamage, refManager.enemyTag.ToString(), refManager.friendlyTag.ToString(), afterEffects);

            for (int j = 0; j < GetActiveModules().Count; j++)
                _projectile.SetModule(GetActiveModules()[j]);
            
        }

        RemoveModules();
        TriggerCooldown();
    }

    Transform GetSpawnPosition()
    {
        if (availableSpawnPositions.Count <= 0)
            ResetSpawnpoints();

        int randomLocation = Random.Range(0, availableSpawnPositions.Count - 1);
        Transform spawnPoint = availableSpawnPositions[randomLocation];
        availableSpawnPositions.Remove(spawnPoint);
        return spawnPoint;
    }

    void ResetSpawnpoints()
    {
        availableSpawnPositions.Clear();
        foreach (Transform point in refManager.SpawnPosition())
        {
            availableSpawnPositions.Add(point);
        }
    }
}
