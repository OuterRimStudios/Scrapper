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
        Sustained,
        FriendlyAI
    };

    public AbilityType abilityType;

    List<ModuleAbility> activeModules = new List<ModuleAbility>();

    [Range(1, 5)]
    public int abilityCharges = 1;
    public float chargeTime;
    public float abilityCooldown;
    public bool requiresTarget;

    public List<AfterEffect> afterEffects;

    int charges;
    bool onCooldown;
    [HideInInspector] public bool isCharging;
    [HideInInspector] public bool isFiring;
    protected bool moduleActive;

    public ReferenceManager refManager;

    Coroutine charge;

    protected virtual void Start()
    {
        charges = abilityCharges;
    }

    public virtual void ActivateAbility()
    {
        if (!isCharging)
        {
            charge = StartCoroutine(Charge());
            charges--;
        }
    }

    public virtual void DeactivateAbility()
    {
        if(charge != null)
        {
            StopCoroutine(charge);
            isCharging = false;
        }
    }
    public virtual void VisualOnCharge() { }
    public virtual void VisualOnActivate() { }
    public virtual void VisualOnDeactivate() { }

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

    protected virtual IEnumerator Charge()
    {
        isCharging = true;
        yield return new WaitForSeconds(chargeTime);
        isCharging = false;
    }

    protected bool CheckIfFriendly()
    {
        if (refManager.friendlyTag.ToString() == "Friendly")
            return true;
        else
            return false;
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