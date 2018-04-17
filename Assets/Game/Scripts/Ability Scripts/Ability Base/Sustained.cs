using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sustained : DamageTypes
{
    public float effectFrequency;
    public float range;
    public float hitRadius;
    public LayerMask visualLayer;

    protected Transform target;
    protected WaitForSeconds effectDelay;
    protected Coroutine repeater;
    protected GameObject mainCam;
    protected bool hitTarget;

    RaycastHit[] hitObjects = new RaycastHit[0];

    protected virtual void Awake()
    {
        if (Camera.main.gameObject)
            mainCam = Camera.main.gameObject;

        effectDelay = new WaitForSeconds(effectFrequency);
        layerMask = 1 << LayerMask.NameToLayer(enemyTag);
    }

    private void Start()
    {
        if (Camera.main.gameObject)
            mainCam = Camera.main.gameObject;
    }

    private void OnEnable()
    {
        if (repeater != null)
            StopCoroutine(repeater);

        repeater = StartCoroutine(RepeatEffect());
    }

    private void OnDisable()
    {
        if (repeater != null)
            StopCoroutine(repeater);
    }

    public virtual void EffectOnTrigger(List<GameObject> objectsHit)
    {
        for (int i = 0; i < objectsHit.Count; i++)
        {
            ApplyModules(objectsHit[i].transform.root.gameObject, true);

            if (objectsHit[i].tag.Equals("Limb"))
                objectsHit[i].GetComponent<Limb>().TookDamage(damage);
            else
                objectsHit[i].GetComponent<Health>().TookDamage(damage);

            VisualOnTrigger();
            SpawnAfterEffects();
        }
        RemoveModules();
    }

    public virtual void EffectOnTrigger(GameObject objectHit)
    {
        ApplyModules(objectHit.transform.root.gameObject, true);

        if (objectHit.tag.Equals("Limb"))
            objectHit.GetComponent<Limb>().TookDamage(damage);
        else
            objectHit.GetComponent<Health>().TookDamage(damage);

        VisualOnTrigger();
        SpawnAfterEffects();
    }

    //Call this whenever you want visual effects to play
    public virtual void VisualOnTrigger()
    {

    }

    public void SetTarget(Transform _target, bool _hitTarget)
    {
        target = _target;
        hitTarget = _hitTarget;
    }

    protected virtual IEnumerator RepeatEffect()
    {
        if (mainCam)
        {
            Ray ray = new Ray(mainCam.transform.position, mainCam.transform.forward);
            if (spawnPos)
                hitObjects = Physics.CapsuleCastAll(spawnPos.position, ray.GetPoint(range), hitRadius, spawnPos.forward, range, layerMask);
        }

        if (hitObjects.Length > 0)
        {
            List<GameObject> hitObjectsList = new List<GameObject>();
            foreach (RaycastHit hit in hitObjects)
            {
                if (hit.transform.gameObject != null)
                {
                    if (!hitObjectsList.Contains(hit.transform.gameObject))
                    {
                        if (hit.transform.gameObject.activeInHierarchy)
                            if (hit.transform.gameObject.layer == LayerMask.NameToLayer(enemyTag))
                                hitObjectsList.Add(hit.transform.gameObject);
                    }
                }
            }

            EffectOnTrigger(hitObjectsList);
        }

        yield return effectDelay;

        RestartEffect();
    }

    void RestartEffect()
    {
        StopCoroutine(repeater);
        repeater = StartCoroutine(RepeatEffect());
    }

    public void UpdateHitRadius(float newHitRadius)
    {
        hitRadius = newHitRadius;
    }
}
