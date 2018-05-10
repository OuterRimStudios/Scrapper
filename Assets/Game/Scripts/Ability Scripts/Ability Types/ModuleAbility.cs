using System.Collections;
using UnityEngine;

public class ModuleAbility : Ability
{
    public enum Module
    {
        Concussion,
        Incinerating,
        Crippling,
        Weighted,
        Siphon,
        Hemorrhage
    };

    public Module module;
    public int damage;
    public float effectLength;
    public float pushForce;
    public int slowAmount;
    public int leechPercentage;
    public int stackAmount;
    InputManager inputManager;

    private void Awake()
    {
        inputManager = GameObject.Find("Player").GetComponent<InputManager>();
    }

    public override void ActivateAbility()
    {
        if (moduleActive) return;
        moduleActive = true;
        base.ActivateAbility();
        ActivateModule();
        RemoveModules();
        TriggerCooldown();
    }

    void ActivateModule()
    {
        for(int i = 0; i < AbilityManager.instance.currentLoadout.ActiveAbilities.Count; i++)
        {
            if(AbilityManager.instance.currentLoadout.ActiveAbilities[i])
                AbilityManager.instance.currentLoadout.ActiveAbilities[i].ModuleActivated(this);
        }
    }
}