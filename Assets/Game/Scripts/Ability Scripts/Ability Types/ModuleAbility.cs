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
        for(int i = 0; i < inputManager.abilities.Count; i++)
        {
            if (inputManager.abilities[i])
                inputManager.abilities[i].ModuleActivated(this);
        }

        //if(inputManager.abilityOne)
        //    inputManager.abilityOne.ModuleActivated(this);
        //if (inputManager.abilityTwo)
        //    inputManager.abilityTwo.ModuleActivated(this);
        //if (inputManager.abilityThree)
        //    inputManager.abilityThree.ModuleActivated(this);
        //if (inputManager.abilityFour)
        //    inputManager.abilityFour.ModuleActivated(this);
        //if (inputManager.abilityFive)
        //    inputManager.abilityFive.ModuleActivated(this);
    }
}