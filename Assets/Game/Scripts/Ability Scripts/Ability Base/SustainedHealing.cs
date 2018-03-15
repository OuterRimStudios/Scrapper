using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SustainedHealing : Sustained
{
    public int healAmount;

    public override void EffectOnTrigger(GameObject objectHit)
    {
        print(" >>>>>>>> Healing: " + objectHit.name + " <<<<<<<< ");

        if (objectHit.tag.Equals("Limb"))
            objectHit.transform.root.GetComponent<Health>().Heal(healAmount);
        else
            objectHit.GetComponent<Health>().Heal(healAmount);

        VisualOnTrigger();
        SpawnAfterEffects();
    }
}
