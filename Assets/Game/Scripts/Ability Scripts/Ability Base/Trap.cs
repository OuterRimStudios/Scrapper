using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : DamageTypes
{
    public bool destroyedOnTrigger;
    public float effectFrequency;
    public float hitRadius;
    public LayerMask layerMask;

    WaitForSeconds effectDelay;

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

    private void Start()
    {
        effectDelay = new WaitForSeconds(effectFrequency);
        if (!destroyedOnTrigger)
            StartCoroutine(RepeatEffect());
    }

    IEnumerator RepeatEffect()
    {
        print("Repeating");

        RaycastHit[] hitObjects = Physics.SphereCastAll(transform.position, hitRadius, transform.position, 0, layerMask);

        foreach(RaycastHit hit in hitObjects)
        {
            print("Hit Enemy " + activeModules.Count);
            EffectOnTrigger(hit.transform.gameObject);
        }

        yield return effectDelay;

        StartCoroutine(RepeatEffect());
    }

    public void OnTriggerEnter(Collider other)
    {
        if (!destroyedOnTrigger) return;

        if (other.tag.Equals("Enemy"))
        {
            EffectOnTrigger(other.gameObject);
            
            Destroy(gameObject);
        }
    }
}