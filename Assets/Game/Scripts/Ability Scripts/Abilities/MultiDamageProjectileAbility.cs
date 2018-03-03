using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiDamageProjectileAbility : Ability
{
    public Projectile projectile;
    public int initialDamage;
    public int secondaryDamage;

    public override void ActivateAbility()
    {
        base.ActivateAbility();

        Instantiate(projectile, playerManager.SpawnPosition().position, playerManager.SpawnPosition().rotation);
        TriggerCooldown();
    }
}
