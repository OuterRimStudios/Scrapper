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

        Instantiate(projectile, playerManager.SpawnPosition().position, playerManager.SpawnPosition().localRotation);
        TriggerCooldown();
    }
}
