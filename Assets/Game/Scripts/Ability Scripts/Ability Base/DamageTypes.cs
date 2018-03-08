﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTypes : MonoBehaviour
{
    public int damage;
    protected List<AfterEffect> afterEffects = new List<AfterEffect>();
    protected List<ModuleAbility> activeModules = new List<ModuleAbility>();

    protected Transform spawnPos;

    public void SetModule(ModuleAbility module)
    {
        activeModules.Add(module);
    }

    public void Initialize(int _damage)
    {
        damage = _damage;
    }

    public void Initialize(int _damage, List<AfterEffect> _afterEffects)
    {
        damage = _damage;
        afterEffects = _afterEffects;
    }

    public void Initialize(int _damage, List<AfterEffect> _afterEffects, Transform _spawnPos)
    {
        damage = _damage;
        afterEffects = _afterEffects;
        spawnPos = _spawnPos;
    }

    protected virtual void SpawnAfterEffects()
    {
        if (afterEffects.Count > 0)
        {
            for (int i = 0; i < afterEffects.Count; i++)
            {
                for (int j = 0; j < afterEffects[i].effectAmount; j++)
                {
                    GameObject newEffect = Instantiate(afterEffects[i].effect.gameObject, transform.position, transform.rotation);
                    Projectile tempProjectile = newEffect.GetComponent<Projectile>();
                    if (tempProjectile != null)
                        tempProjectile.Initialize(afterEffects[i].effectDamage);
                    else
                    {
                        Trap tempTrap = newEffect.GetComponent<Trap>();
                        if (tempTrap != null)
                            tempTrap.Initialize(afterEffects[i].effectDamage);
                    }
                }
            }
        }
    }

    public void ApplyModules(GameObject other)
    {
        if (activeModules.Count > 0)
        {
            for (int i = 0; i < activeModules.Count; i++)
            {
                switch (activeModules[i].module)
                {
                    case ModuleAbility.Module.Concussion:
                        other.GetComponent<Modules>().Concussion(activeModules[i].effectLength);
                        break;
                    case ModuleAbility.Module.Incinerating:
                        other.GetComponent<Modules>().Incinerating(activeModules[i].damage, activeModules[i].effectLength);
                        break;
                    case ModuleAbility.Module.Crippling:
                        other.GetComponent<Modules>().Crippling(activeModules[i].effectLength, activeModules[i].slowAmount, activeModules[i].stackAmount);
                        break;
                    case ModuleAbility.Module.Weighted:
                        other.GetComponent<Modules>().Weighted(activeModules[i].pushForce);
                        break;
                    case ModuleAbility.Module.Siphon:
                        float leechDamage = damage
                            / activeModules[i].leechPercentage;
                        other.GetComponent<Modules>().Siphon(Mathf.RoundToInt(leechDamage));
                        break;
                    case ModuleAbility.Module.Hemorrhage:
                        other.GetComponent<Modules>().Hemorrhage(activeModules[i].damage, activeModules[i].effectLength, activeModules[i].stackAmount);
                        break;
                }
            }
        }
    }
}
