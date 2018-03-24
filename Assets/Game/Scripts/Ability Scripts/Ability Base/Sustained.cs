using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sustained : DamageTypes {

    public float effectFrequency;
    public float range;
    public float hitRadius;
    
    public LineRenderer beamRenderer;

    protected Transform target;
    protected WaitForSeconds effectDelay;
    protected Coroutine repeater;
    protected GameObject mainCam;
    protected bool isTurret;
    protected bool shootForward;

    protected virtual void Awake()
    {
        mainCam = Camera.main.gameObject;
        effectDelay = new WaitForSeconds(effectFrequency);
        layerMask = 1 << LayerMask.NameToLayer(enemyTag);
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
        ApplyModules(objectHit.transform.root.gameObject);

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

    private void Update()
    {
        beamRenderer.SetPosition(0, spawnPos.position);

        if(!target)
        {
            if (repeater != null)
                StopCoroutine(repeater);

            beamRenderer.enabled = false;
            return;
        }
        else if(target)
        {
            if (!target.gameObject.activeInHierarchy)
            {
                if (repeater != null)
                    StopCoroutine(repeater);

                beamRenderer.enabled = false;
                return;
            }
            beamRenderer.enabled = true;
        }

        if (isTurret && target)                                                              //Turret
            beamRenderer.SetPosition(1, target.position + spawnPos.forward * range);
        else if(shootForward && target)                                                     //AI
            beamRenderer.SetPosition(1, spawnPos.position + spawnPos.forward * range);
        else
        {                                                                                   //Player
            if(mainCam)
            {
                Ray ray = new Ray(mainCam.transform.position, mainCam.transform.forward);
                beamRenderer.SetPosition(1, ray.GetPoint(range));
            }
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

    protected virtual IEnumerator RepeatEffect()
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
        else
        {
            Ray ray = new Ray(mainCam.transform.position, mainCam.transform.forward);
            hitObjects = Physics.CapsuleCastAll(spawnPos.position, ray.GetPoint(range), hitRadius, spawnPos.forward, range, layerMask);
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

        RestartEffect();
    }

    void RestartEffect()
    {
        StopCoroutine(repeater);
        repeater = StartCoroutine(RepeatEffect());
    }
}