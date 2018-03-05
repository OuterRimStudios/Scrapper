using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAbility : Ability
{
    public Projectile projectile;
    public int initialDamage;

    public override void ActivateAbility()
    {
        base.ActivateAbility();
        Projectile _projectile = Instantiate(projectile, playerManager.SpawnPosition().position, playerManager.SpawnPosition().rotation);
        _projectile.Initialize(initialDamage);
   
        for(int i = 0; i < GetActiveModules().Count; i++)
            _projectile.SetModule(GetActiveModules()[i]);
        RemoveModules();
        TriggerCooldown();
    }
}
