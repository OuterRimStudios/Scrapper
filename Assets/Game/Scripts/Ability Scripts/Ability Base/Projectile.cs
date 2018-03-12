using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : DamageTypes
{
    public float speed;
    public bool destroyedOnTrigger;

    Transform target;    

    public virtual void EffectOnTrigger(GameObject objectHit)
    {
        print("Hit Enemy " + activeModules.Count);
        ApplyModules(objectHit);
        objectHit.GetComponent<Health>().TookDamage(damage);
        VisualOnTrigger();
        SpawnAfterEffects();
    }

    //Call this whenever you want visual effects to play
    public virtual void VisualOnTrigger()
    {

    }

    private void Update()
    {
        ProjectileMovement();
    }

    public virtual void ProjectileMovement()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals(enemyTag))
        {
            //print("Hit Enemy " + activeModules.Count);
            //ApplyModules(other.gameObject);
            //other.GetComponent<Health>().TookDamage(damage);

            EffectOnTrigger(other.gameObject);

            if (destroyedOnTrigger)
                Destroy(gameObject);
        }        
    }
}