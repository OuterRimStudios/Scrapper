using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terminal : MonoBehaviour
{
    InputManager inputManager;
    AbilityItem terminalAbility;
    AbilityDisplayArea abilityDisplayArea;

    private void Start()
    {
        abilityDisplayArea = AbilityDisplayArea.instance;
        terminalAbility = LootWrangler.instance.GetRandomAbility();
        abilityDisplayArea.SetNewItemSlot(terminalAbility.ability);

        if (terminalAbility.ability.abilityType == Ability.AbilityType.Mobility)
            abilityDisplayArea.movementAbility = true;
        else
            abilityDisplayArea.movementAbility = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.name.Equals("Player"))
        {
            if (!inputManager)
                inputManager = other.GetComponent<InputManager>();

            inputManager.canInteract = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.name.Equals("Player"))
        {
            if (!inputManager)
                inputManager = other.GetComponent<InputManager>();

            inputManager.canInteract = false;
        }
    }
}