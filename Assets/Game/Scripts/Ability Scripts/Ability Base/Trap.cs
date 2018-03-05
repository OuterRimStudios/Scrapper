using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    public int damage;
    public bool destroyedOnTrigger;

    List<ModuleAbility> activeModules = new List<ModuleAbility>();

    public void SetModule(ModuleAbility module)
    {
        activeModules.Add(module);
    }

    public virtual void EffectOnTrigger(GameObject objectHit)
    {

    }

    Transform target;

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Enemy"))
        {
            print("Hit Enemy " + activeModules.Count);
            ApplyModules(other.gameObject);
            //other.GetComponent<Health>().TookDamage(damage);
            EffectOnTrigger(other.gameObject);
            if (destroyedOnTrigger)
                Destroy(gameObject);
        }
    }

    void ApplyModules(GameObject other)
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
