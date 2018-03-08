using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sustained : DamageTypes {

    public float effectFrequency;
    public float range;
    public float hitRadius;
    public LayerMask layerMask;

    WaitForSeconds effectDelay;
    Coroutine repeater;

    private void Start()
    {
        effectDelay = new WaitForSeconds(effectFrequency);
    }

    private void OnEnable()
    {
        repeater = StartCoroutine(RepeatEffect());
    }

    private void OnDisable()
    {
        if(repeater != null)
            StopCoroutine(repeater);
    }

    public virtual void EffectOnTrigger(GameObject objectHit)
    {
        print(objectHit.name);
        ApplyModules(objectHit);
        objectHit.GetComponent<Health>().TookDamage(damage);
        VisualOnTrigger();
        SpawnAfterEffects();
    }

    //Call this whenever you want visual effects to play
    public virtual void VisualOnTrigger()
    {

    }

    IEnumerator RepeatEffect()
    {
        RaycastHit[] hitObjects = Physics.CapsuleCastAll(spawnPos.position, spawnPos.forward * range, hitRadius, spawnPos.forward, layerMask);

        foreach (RaycastHit hit in hitObjects)
        {
            EffectOnTrigger(hit.transform.gameObject);
        }

        yield return effectDelay;

        StartCoroutine(RepeatEffect());
    }
}