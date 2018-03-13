using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sustained : DamageTypes {

    public float effectFrequency;
    public float range;
    public float hitRadius;
    
    public LineRenderer beamRenderer;

    Transform target;
    WaitForSeconds effectDelay;
    Coroutine repeater;
    GameObject mainCam;
    bool isTurret;
    bool shootForward;

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
        if(isTurret && target)
            beamRenderer.SetPosition(1, target.position + spawnPos.forward * range);
        else if(shootForward && target)
            beamRenderer.SetPosition(1, spawnPos.position + spawnPos.forward * range);
        else if(target)
        {
            print("Set Posss");
            beamRenderer.SetPosition(1, spawnPos.position + transform.forward * range);
        }

        beamRenderer.startWidth = hitRadius;
        beamRenderer.endWidth = hitRadius;
    }

    public void SetTarget(Transform _target, bool _isTurret, bool _shootForward)
    {
        target = _target;
        isTurret = _isTurret;
        shootForward = _shootForward;
    }

    IEnumerator RepeatEffect()
    {
        RaycastHit[] hitObjects = new RaycastHit[0];
        if (isTurret && target)
        {
            hitObjects = Physics.CapsuleCastAll(spawnPos.position, target.position, hitRadius, spawnPos.forward, range, layerMask);
        }
        else if(shootForward && target)
        {
            hitObjects = Physics.CapsuleCastAll(spawnPos.position, spawnPos.position, hitRadius, spawnPos.forward, range, layerMask);
        }
        else if (target)
        {
            print("Fireee");
            hitObjects = Physics.CapsuleCastAll(spawnPos.position, target.forward * range, hitRadius, spawnPos.forward, range, layerMask);
        }

        if (hitObjects.Length > 0)
        {
            List<GameObject> hitObjectsList = new List<GameObject>();
            foreach (RaycastHit hit in hitObjects)
            {
                if (!hitObjectsList.Contains(hit.transform.gameObject))
                {
                    hitObjectsList.Add(hit.transform.gameObject);
                    if (hit.transform.gameObject.layer == LayerMask.NameToLayer(enemyTag))
                        EffectOnTrigger(hit.transform.gameObject);
                }
            }
        }

        yield return effectDelay;

        StartCoroutine(RepeatEffect());
    }
}