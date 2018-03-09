using UnityEngine;

public class StunTrap : Trap {

    public float stunDuration;

    public override void EffectOnTrigger(GameObject objectHit)
    {
        base.EffectOnTrigger(objectHit);
        objectHit.GetComponent<StatusEffects>().ApplyStun(stunDuration);
    }
}