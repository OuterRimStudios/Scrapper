using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileTurret : Turret
{
    public float effectFrequency;
    public bool alternatingFire;

    bool firing;
    WaitForSeconds fireRate;

    int alternateFireCount;

    private void Awake()
    {
        fireRate = new WaitForSeconds(effectFrequency);
    }

    public override void Update()
    {
        base.Update();
        if (!target) return;
        if(!firing)
        {
            firing = true;
            StartCoroutine(Firing());
        }
    }

    IEnumerator Firing()
    {
        if (alternatingFire)
        {
            Projectile newProjectile = Instantiate(turretAbility, spawnpoints[alternateFireCount].transform.position, spawnpoints[alternateFireCount].transform.rotation) as Projectile;
            AlternatingFire(spawnpoints.Length);

            newProjectile.Initialize(damage, enemyTag, friendlyTag, afterEffects);

            for (int j = 0; j < GetActiveModules().Count; j++)
                newProjectile.SetModule(GetActiveModules()[j]);
            RemoveModules();
        }
        else
        {
            for (int i = 0; i < spawnpoints.Length; i++)
            {
                Projectile newProjectile = Instantiate(turretAbility, spawnpoints[i].transform.position, spawnpoints[i].transform.rotation) as Projectile;

                newProjectile.Initialize(damage, enemyTag, friendlyTag, afterEffects);

                for (int j = 0; j < GetActiveModules().Count; j++)
                    newProjectile.SetModule(GetActiveModules()[j]);
                RemoveModules();
            }
        }


        yield return fireRate;
        firing = false;
    }

    public void AlternatingFire(int weaponFirePositionCount)
    {
        alternateFireCount++;

        if (alternateFireCount > weaponFirePositionCount - 1)
            alternateFireCount = 0;
    }
}
