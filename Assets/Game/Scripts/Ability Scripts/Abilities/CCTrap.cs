using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCTrap : Trap
{
    public float ccLength;
    public GameObject triggerVisual;

    public override void EffectOnTrigger(GameObject objectHit)
    {
        base.EffectOnTrigger(objectHit);
        objectHit.transform.root.GetComponent<StatusEffects>().ApplyCC(ccLength);
    }

    public override void VisualOnTrigger()
    {
        Instantiate(triggerVisual, transform.position, Quaternion.identity);
    }
}
