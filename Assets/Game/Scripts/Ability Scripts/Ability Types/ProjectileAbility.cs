public class ProjectileAbility : Ability
{
    public Projectile projectile;
    public int initialDamage;

    public override void ActivateAbility()
    {
        base.ActivateAbility();
        Projectile _projectile = Instantiate(projectile, refManager.SpawnPosition()[0].position, refManager.SpawnPosition()[0].rotation);
        _projectile.Initialize(initialDamage, refManager.enemyTag.ToString(), refManager.friendlyTag.ToString(), afterEffects);

        if (abilityInput != AbilityInput.AIControlled)
        {
            for (int j = 0; j < GetActiveModules().Count; j++)
                _projectile.SetModule(GetActiveModules()[j]);

            RemoveModules();
        }

        TriggerCooldown();
    }
}