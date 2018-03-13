using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTypes : MonoBehaviour
{
    public int damage;
    public LayerMask layerMask;
    protected List<AfterEffect> afterEffects = new List<AfterEffect>();
    protected List<ModuleAbility> activeModules = new List<ModuleAbility>();

    protected Transform spawnPos;
    protected string enemyTag;

    public void SetModule(ModuleAbility module)
    {
        activeModules.Add(module);
    }

    public void Initialize(int _damage, string _enemyTag)
    {
        damage = _damage;
        enemyTag = _enemyTag;
        layerMask = 1 << LayerMask.NameToLayer(enemyTag);
    }

    public void Initialize(int _damage, string _enemyTag, List<AfterEffect> _afterEffects)
    {
        Initialize(_damage, _enemyTag);
        afterEffects = _afterEffects;
    }

    public void Initialize(int _damage, string _enemyTag, List<AfterEffect> _afterEffects, Transform _spawnPos)
    {
        Initialize(_damage, _enemyTag, _afterEffects);
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
                        tempProjectile.Initialize(afterEffects[i].effectDamage, enemyTag);
                    else
                    {
                        Trap tempTrap = newEffect.GetComponent<Trap>();
                        if (tempTrap != null)
                            tempTrap.Initialize(afterEffects[i].effectDamage, enemyTag);
                    }
                }
            }
        }
    }

    public void RemoveModules()
    {
        activeModules.Clear();
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
                        other.GetComponent<StatusEffects>().ApplyStun(activeModules[i].effectLength);
                        break;
                    case ModuleAbility.Module.Incinerating:
                        other.GetComponent<StatusEffects>().ApplyDOT(activeModules[i].damage, activeModules[i].effectLength);
                        break;
                    case ModuleAbility.Module.Crippling:
                        other.GetComponent<StatusEffects>().StackSlow(activeModules[i].effectLength, activeModules[i].slowAmount, activeModules[i].stackAmount);
                        break;
                    case ModuleAbility.Module.Weighted:
                        other.GetComponent<StatusEffects>().KnockedBack(activeModules[i].pushForce);
                        break;
                    case ModuleAbility.Module.Siphon:
                        float leechDamage = damage
                            / activeModules[i].leechPercentage;
                        other.GetComponent<StatusEffects>().Siphon(Mathf.RoundToInt(leechDamage));
                        break;
                    case ModuleAbility.Module.Hemorrhage:
                        other.GetComponent<StatusEffects>().StackDot(activeModules[i].damage, activeModules[i].effectLength, activeModules[i].stackAmount);
                        break;
                }
            }
            RemoveModules();
        }
    }
}
