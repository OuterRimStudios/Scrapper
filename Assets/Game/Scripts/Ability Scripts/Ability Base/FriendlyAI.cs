using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendlyAI : DamageTypes
{
    public DamageTypes aiAbility;
    public Transform[] spawnpoints;
    public float scanForTargetFrequency;
    public float despawnAfter;
    public bool scanForTarget;
    public bool targetsFriendlies;

    protected Transform target;

    bool scanningForTarget;
    WaitForSeconds scan;
    WaitForSeconds despawnTime;

    TargetManager targetManager;
    SpawnAIAbility ability;

    bool moduleActive;

    protected virtual void Start()
    {
        targetManager = GameObject.Find("GameManager").GetComponent<TargetManager>();
        scan = new WaitForSeconds(scanForTargetFrequency);
        despawnTime = new WaitForSeconds(despawnAfter);
        StartCoroutine(DespawnAfter());
    }

    public void Initialize(SpawnAIAbility _ability)
    {
        ability = _ability;
    }

    IEnumerator DespawnAfter()
    {
        yield return despawnTime;
        ability.RemoveFriendlyAI(this);
        Destroy(gameObject);
    }

    public virtual void Update()
    {
        if (!scanForTarget) return;

        if (!scanningForTarget)
        {
            scanningForTarget = true;
            StartCoroutine(Scan());
        }
    }

    IEnumerator Scan()
    {
        if(targetsFriendlies)
            target = targetManager.GetClosestTarget(transform.position, friendlyTag);
        else
            target = targetManager.GetClosestTarget(transform.position, enemyTag);

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
