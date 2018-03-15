using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAIAbility : Ability
{
    public FriendlyAI aiToSpawn;
    public Transform[] spawnpoints;
    public int amountToSpawn;

    public List<FriendlyAI> activeAIs;

    public override void ActivateAbility()
    {
        base.ActivateAbility();

        for (int i = 0; i < amountToSpawn; i++)
        {
            int spawnIndex = (i + 1) % spawnpoints.Length;
            FriendlyAI ai = Instantiate(aiToSpawn, spawnpoints[spawnIndex].position, spawnpoints[spawnIndex].rotation);
            activeAIs.Add(ai);

            for (int j = 0; j < GetActiveModules().Count; j++)
                ai.SetModule(GetActiveModules()[i]);
        }

        RemoveModules();
        TriggerCooldown();
    }

    public override void ModuleActivated(ModuleAbility module)
    {
        base.ModuleActivated(module);
        print("AI Placed Module Activated");
        if (abilityType == AbilityType.FriendlyAI)
            for (int i = 0; i < activeAIs.Count; i++)
                for (int j = 0; j < GetActiveModules().Count; j++)
                {
                    activeAIs[i].ModuleActivated(module);
                    activeAIs[i].SetModule(GetActiveModules()[j]);
                }
    }

    public void RemoveFriendlyAI(FriendlyAI ai)
    {
        if (activeAIs.Contains(ai))
            activeAIs.Remove(ai);
    }
}
