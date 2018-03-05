using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunProjectile : Projectile {

    public float stunDuration;

    public override void EffectOnTrigger(GameObject objectHit)
    {
        base.EffectOnTrigger(objectHit);
        objectHit.GetComponent<AI>().ApplyStun(stunDuration);
    }
}