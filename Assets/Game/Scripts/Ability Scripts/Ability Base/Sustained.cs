using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sustained : DamageTypes {

    public float effectFrequency;
    public float range;
    public float hitRadius;
    public LayerMask layerMask;
    public LineRenderer beamRenderer;

    Transform target;
    WaitForSeconds effectDelay;
    Coroutine repeater;
    GameObject mainCam;
    bool isTurret;

    private void Awake()
    {
        mainCam = Camera.main.gameObject;
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
        if(isTurret)
            beamRenderer.SetPosition(1, target.position + spawnPos.forward * range);
        else
            beamRenderer.SetPosition(1, spawnPos.position + target.forward * range);
        beamRenderer.startWidth = hitRadius;
        beamRenderer.endWidth = hitRadius;
    }

    public void SetTarget(Transform _target, bool _isTurret)
    {
        target = _target;
        isTurret = _isTurret;
    }

    IEnumerator RepeatEffect()
    {
        RaycastHit[] hitObjects;
        if (isTurret)
        {
            hitObjects = Physics.CapsuleCastAll(spawnPos.position, target.position, hitRadius, spawnPos.forward, range, layerMask);
        }
        else
        {
            hitObjects = Physics.CapsuleCastAll(spawnPos.position, target.forward * range, hitRadius, spawnPos.forward, range, layerMask);
        }

        foreach (RaycastHit hit in hitObjects)
        {
            print("Hit " + hit.transform.name);
            EffectOnTrigger(hit.transform.gameObject);
        }

        yield return effectDelay;

        StartCoroutine(RepeatEffect());
    }
}