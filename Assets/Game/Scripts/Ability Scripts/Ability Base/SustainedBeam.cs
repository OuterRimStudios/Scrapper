using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SustainedBeam : DamageTypes {

    public float effectFrequency;
    public float range;
    public float hitRadius;
    public LayerMask visualLayer;
    
    public LineRenderer beamRenderer;

    protected Transform target;
    protected WaitForSeconds effectDelay;
    protected Coroutine repeater;
    protected GameObject mainCam;
    protected bool hitTarget;

    [Space, Header("Visual Variables")]
    public GameObject muzzleFlash;
    public GameObject hitEffect;

    public float beamEndOffset = 1f; //How far from the raycast hit point the end effect is positioned
    public float textureScrollSpeed = 8f; //How fast the texture scrolls along the beam
    public float textureLengthScale = 3; //Length of the beam texture
    RaycastHit[] hitObjects = new RaycastHit[0];


    protected virtual void Awake()
    {
        if(Camera.main.gameObject)
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
        if(repeater != null)
            StopCoroutine(repeater);
    }

    public virtual void EffectOnTrigger(List<GameObject> objectsHit)
    {
        for(int i = 0; i < objectsHit.Count; i++)
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

    private void Update()
    {
        //if (!target)
        //{
        //    if (repeater != null)
        //        StopCoroutine(repeater);
            
        //    return;
        //}
        //else if(target)
        //{
        //    if (!target.gameObject.activeInHierarchy)
        //    {
        //        if (repeater != null)
        //            StopCoroutine(repeater);
        //        return;
        //    }
        //    else
        //    {
        //        if(repeater == null)
        //        {
        //            repeater = StartCoroutine(RepeatEffect());
        //        }
        //    }
        //}

        Ray ray = new Ray(spawnPos.position, spawnPos.forward);
        if(transform.root.name == "Player")
        {
            if (mainCam)
            {
                RaycastHit hit;
                if (Physics.Raycast(ray.origin, ray.direction, out hit, range, visualLayer))
                    ShootBeamInDir(spawnPos.position, mainCam.transform.forward, hit.point);
                else
                    ShootBeamInDir(spawnPos.position, mainCam.transform.forward, ray.GetPoint(range));
            }
        }
        else if (!hitTarget && target)                                                             //AI  
        {
            ShootBeamInDir(spawnPos.position, spawnPos.forward, ray.GetPoint(range));
        }
        else if(hitTarget && target) //Turret
        {
                RaycastHit hit;
                if (Physics.Raycast(ray.origin, ray.direction, out hit, range, visualLayer))
                    ShootBeamInDir(spawnPos.position, spawnPos.forward, hit.point);
                else
                    ShootBeamInDir(spawnPos.position, spawnPos.forward, ray.GetPoint(range));
        }

        beamRenderer.startWidth = hitRadius;
        beamRenderer.endWidth = hitRadius; 
    }

    public void SetTarget(Transform _target, bool _hitTarget)
    {
        target = _target;
        hitTarget = _hitTarget;
    }

    protected virtual IEnumerator RepeatEffect()
    {
        if (hitTarget && target)
        {
            hitObjects = Physics.CapsuleCastAll(spawnPos.position, target.position, hitRadius, spawnPos.forward, range, layerMask);
        }
        else if(!hitTarget && target)
        {
            hitObjects = Physics.CapsuleCastAll(spawnPos.position, spawnPos.position, hitRadius, spawnPos.forward, range, layerMask);
        }
        else
        {
            if (mainCam)
            {
                Ray ray = new Ray(mainCam.transform.position, mainCam.transform.forward);
                if(spawnPos)
                hitObjects = Physics.CapsuleCastAll(spawnPos.position, ray.GetPoint(range), hitRadius, spawnPos.forward, range, layerMask);
            }
        }
        if (hitObjects.Length > 0)
        {
            List<GameObject> hitObjectsList = new List<GameObject>();
            foreach (RaycastHit hit in hitObjects)
            {
                if(hit.transform.gameObject != null)
                {
                    if (!hitObjectsList.Contains(hit.transform.gameObject))
                    {
                        if(hit.transform.gameObject.activeInHierarchy)
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

    void ShootBeamInDir(Vector3 start, Vector3 dir, Vector3 end)
    {
        muzzleFlash.transform.position = start;
        beamRenderer.SetPosition(0, muzzleFlash.transform.localPosition);     
        
        hitEffect.transform.position = end;

        beamRenderer.SetPosition(1, hitEffect.transform.localPosition);

        muzzleFlash.transform.LookAt(hitEffect.transform.position);
        hitEffect.transform.LookAt(muzzleFlash.transform.position);

        float distance = Vector3.Distance(start, end);
        beamRenderer.sharedMaterial.mainTextureScale = new Vector2(distance / textureLengthScale, 1);
        beamRenderer.sharedMaterial.mainTextureOffset -= new Vector2(Time.deltaTime * textureScrollSpeed, 0);
    }

    public void UpdateHitRadius(float newHitRadius)
    {
        hitRadius = newHitRadius;
    }
}