using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootTrap : Trap
{
    public float rootLength;

    public override void EffectOnTrigger(GameObject objectHit)
    {
        objectHit.GetComponent<AI>().ApplyRoot(rootLength);
    }
}
