using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : DamageTypes
{
    public bool destroyedOnTrigger;

    public virtual void EffectOnTrigger(GameObject objectHit)
    {

    }

    Transform target;

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Enemy"))
        {
            print("Hit Enemy " + activeModules.Count);
            ApplyModules(other.gameObject);
            //other.GetComponent<Health>().TookDamage(damage);
            EffectOnTrigger(other.gameObject);

            if (destroyedOnTrigger)
                Destroy(gameObject);
        }
    }
}
