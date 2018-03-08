using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sustained : DamageTypes {

    public float effectFrequency;
    public float range;
    public float hitRadius;
    public LayerMask layerMask;
    public LineRenderer beamRenderer;

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

    private void Update()
    {
        beamRenderer.SetPosition(0, spawnPos.position);
        beamRenderer.SetPosition(1, spawnPos.position + Camera.main.transform.forward * range);
        beamRenderer.startWidth = hitRadius;
        beamRenderer.endWidth = hitRadius;
    }

    IEnumerator RepeatEffect()
    {
        RaycastHit[] hitObjects = Physics.CapsuleCastAll(spawnPos.position, spawnPos.forward, hitRadius, spawnPos.forward, range, layerMask);

        foreach (RaycastHit hit in hitObjects)
        {
            print("Hit " + hit.transform.name);
            EffectOnTrigger(hit.transform.gameObject);
        }

        yield return effectDelay;

        StartCoroutine(RepeatEffect());
    }
}