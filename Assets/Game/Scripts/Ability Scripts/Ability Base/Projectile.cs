using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
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
}
