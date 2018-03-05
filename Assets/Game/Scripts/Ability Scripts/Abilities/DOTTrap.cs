﻿using UnityEngine;

public class DOTTrap : Trap {

    public int dotDamage;
    public float dotDuration;

    public override void EffectOnTrigger(GameObject objectHit)
    {
        base.EffectOnTrigger(objectHit);
        objectHit.GetComponent<Health>().ApplyDOT(dotDamage, dotDuration);
    }
}