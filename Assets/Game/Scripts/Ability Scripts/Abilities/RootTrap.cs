using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootTrap : Trap
{
    public float rootLength;
    public GameObject triggerVisual;

    public override void EffectOnTrigger(GameObject objectHit)
    {
        base.EffectOnTrigger(objectHit);
        objectHit.transform.root.GetComponent<StatusEffects>().ApplyRoot(rootLength);
    }

    public override void VisualOnTrigger()
    {
        Instantiate(triggerVisual, transform.position, Quaternion.identity);
    }
}
