using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiProjectileAmmoAbility : Ability, IReloadable
{
    public Projectile projectile;
    public List<Transform> spawnPositions;
    public int maxAmmo;
    public float reloadTime = 3;
    public int initialDamage;
    public bool alternateFire;
    public bool unlimitedAmmo;
    int ammo;
    bool reloading;

    List<Transform> availableSpawnPositions = new List<Transform>();

    protected override void Start()
    {
        base.Start();
        ResetSpawnpoints();
        ammo = maxAmmo;
    }
    private void Reload()
    {
        ammo = maxAmmo;
        print("Reloaded");
    }

    public bool IsReloading()
    {
        return reloading;
    }

    IEnumerator Reloading()
    {
        print("Reloading... ");
        yield return new WaitForSeconds(reloadTime);
        Reload();
        reloading = false;
    }

    public override void ActivateAbility()
    {
        if (CanShoot() && !IsReloading())
        {
            base.ActivateAbility();

            if(!unlimitedAmmo)
                ammo--;

            print("Firing " + abilityName);

            if (!alternateFire)
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

            TriggerCooldown();

            if (ammo <= 0)
            {
                reloading = true;
                StartCoroutine(Reloading());
            }
            TriggerCooldown();
        }
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
        foreach (Transform point in spawnPositions)
        {
            availableSpawnPositions.Add(point);
        }
    }
}
