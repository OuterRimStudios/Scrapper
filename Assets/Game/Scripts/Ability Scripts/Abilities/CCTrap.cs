using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCTrap : Trap
{
    public float ccLength;

    public override void EffectOnTrigger(GameObject objectHit)
    {
        objectHit.GetComponent<AI>().ApplyCC(ccLength);
    }
}
