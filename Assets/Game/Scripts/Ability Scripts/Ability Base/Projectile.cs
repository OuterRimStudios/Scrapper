using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : DamageTypes
{
    public float speed;

    Transform target;

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
        if(other.tag.Equals("Enemy"))
        {
            print("Hit Enemy " + activeModules.Count);
            ApplyModules(other.gameObject);
            other.GetComponent<Health>().TookDamage(damage);
        }
    }
}
