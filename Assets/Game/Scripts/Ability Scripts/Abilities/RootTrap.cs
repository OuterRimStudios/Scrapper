using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootTrap : Trap
{
    public float rootLength;

    public override void EffectOnTrigger(GameObject objectHit)
    {
        base.EffectOnTrigger(objectHit);

        StatusEffects effects = objectHit.transform.root.GetComponent<StatusEffects>();
        effects.ApplyRoot(rootLength);
        effects.ActivateContainmentZone(rootLength);
    }
}
