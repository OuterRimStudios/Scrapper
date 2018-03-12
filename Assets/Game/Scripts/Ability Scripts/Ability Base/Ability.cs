using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : MonoBehaviour
{
    public string abilityName;
    public Sprite abilityIcon;
    [TextArea]
    public string abilityDescription;

    public enum AbilityType
    {
        Projectile,
        Trap,
        Module,
        Turret,
        Sustained
    };

    public AbilityType abilityType;

    List<ModuleAbility> activeModules = new List<ModuleAbility>();

    [Range(1, 5)]
    public int abilityCharges = 1;
    public float abilityCooldown;
    public bool requiresTarget;

    public List<AfterEffect> afterEffects;

    int charges;
    bool onCooldown;
    protected bool moduleActive;

    public ReferenceManager refManager;

    protected virtual void Start()
    {
        charges = abilityCharges;
    }

    public virtual void ActivateAbility()
    {
        charges--;
    }

    public virtual void DeactivateAbility()
    {

    }

    public virtual void ModuleActivated(ModuleAbility module)
    {
        activeModules.Add(module);
    }

    public List<ModuleAbility> GetActiveModules()
    {
        return activeModules;
    }

    //This should be called in the custom ability script
    public void RemoveModules()
    {
        activeModules.Clear();
        moduleActive = false;
    }

    //This should be called in the custom ability script
    public void TriggerCooldown()   
    {
        onCooldown = true;
        StartCoroutine(Cooldown());
    }

    public bool CanShoot()
    {
        if (charges > 0)
            return true;
        else return false;
    }

    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(abilityCooldown);

        if (charges < abilityCharges)
            charges++;

        onCooldown = false;
    }
}

[System.Serializable]
public class AfterEffect
{
    public Behaviour effect;
    [Tooltip("The amount of this type of effect that spawns")]
    public int effectAmount;
    public int effectDamage;
}