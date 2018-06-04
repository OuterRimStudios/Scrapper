using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terminal : MonoBehaviour
{
    InputManager inputManager;
    AbilityItem terminalAbility;

    private void Start()
    {
        terminalAbility = LootWrangler.instance.GetRandomAbility();
        AbilityDisplayArea.instance.SetNewItemSlot(terminalAbility.ability);
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