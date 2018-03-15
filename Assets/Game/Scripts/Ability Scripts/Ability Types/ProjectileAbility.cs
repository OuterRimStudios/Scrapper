public class ProjectileAbility : Ability
{
    public Projectile projectile;
    public int initialDamage;

    public override void ActivateAbility()
    {
        base.ActivateAbility();
        for (int i = 0; i < refManager.SpawnPosition().Length; i++)
        {
            Projectile _projectile = Instantiate(projectile, refManager.SpawnPosition()[i].position, refManager.SpawnPosition()[i].rotation);
            _projectile.Initialize(initialDamage, refManager.enemyTag.ToString(), refManager.friendlyTag.ToString(), afterEffects);

            for (int j = 0; j < GetActiveModules().Count; j++)
                _projectile.SetModule(GetActiveModules()[j]);
        }

        RemoveModules();
        TriggerCooldown();
    }
}