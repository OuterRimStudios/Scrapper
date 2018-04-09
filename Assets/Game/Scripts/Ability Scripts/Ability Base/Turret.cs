using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : DamageTypes
{
    public DamageTypes turretAbility;
    public Transform shaft;
    public Transform[] spawnpoints;
    public float scanForTargetFrequency;
    public float despawnAfter;
    public bool scanForTarget;

    protected Transform target;

    bool scanningForTarget;
    WaitForSeconds scan;
    WaitForSeconds despawnTime;

    TargetManager targetManager;
    PlaceTurretAbility ability;

    bool moduleActive;

    private void Awake()
    {
        layerMask = 1 << LayerMask.NameToLayer(enemyTag);
    }

    protected virtual void Start()
    {
        targetManager = GameObject.Find("GameManager").GetComponent<TargetManager>();
        scan = new WaitForSeconds(scanForTargetFrequency);
        despawnTime = new WaitForSeconds(despawnAfter);
        StartCoroutine(DespawnAfter());
    }

    public void Initialize(PlaceTurretAbility _ability)
    {
        ability = _ability;
    }

    IEnumerator DespawnAfter()
    {
        yield return despawnTime;
        ability.RemoveTurret(this);
        Destroy(gameObject);
    }

    public virtual void Update()
    {
        if (!scanForTarget) return;

        if(!scanningForTarget)
        {
            scanningForTarget = true;
            StartCoroutine(Scan());
        }

        if(target)
        {
            Vector3 targetPosition = new Vector3(target.position.x, shaft.transform.position.y, target.position.z);
            shaft.transform.LookAt(targetPosition);
        }
    }

    IEnumerator Scan()
    {
        Transform tempTarget = targetManager.GetClosestTarget(transform.position, enemyTag);

        if (tempTarget.gameObject.activeInHierarchy)
            target = tempTarget;

        TargetUpdated();
        yield return scan;
        scanningForTarget = false;
    }

    protected virtual void TargetUpdated()
    {
    }

    public virtual void ModuleActivated(ModuleAbility module)
    {
        activeModules.Add(module);
    }

    public List<ModuleAbility> GetActiveModules()
    {
        return activeModules;
    }
}
