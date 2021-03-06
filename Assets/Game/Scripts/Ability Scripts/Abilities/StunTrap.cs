﻿using UnityEngine;

public class StunTrap : Trap {

    public float stunDuration;

    public override void EffectOnTrigger(GameObject objectHit)
    {
        base.EffectOnTrigger(objectHit);
        objectHit.transform.root.GetComponent<StatusEffects>().ApplyStun(stunDuration);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, hitRadius);
        Gizmos.color = Color.red;
    }
}