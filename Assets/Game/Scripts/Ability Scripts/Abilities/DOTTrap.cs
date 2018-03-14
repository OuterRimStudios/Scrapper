using UnityEngine;

public class DOTTrap : Trap {

    public int dotDamage;
    public float dotDuration;

    public override void EffectOnTrigger(GameObject objectHit)
    {
        base.EffectOnTrigger(objectHit);
        objectHit.transform.root.GetComponent<StatusEffects>().ApplyDOT(dotDamage, dotDuration);
    }
}