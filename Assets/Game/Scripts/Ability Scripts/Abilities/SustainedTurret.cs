using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SustainedTurret : Turret {

    public float effectLength;
    public float effectCooldown;

    bool firing;
    List<Sustained> beams = new List<Sustained>();

    protected override void Start()
    {
        base.Start();
        for (int i = 0; i < spawnpoints.Length; i++)
        {
            beams.Add(Instantiate(turretAbility) as Sustained);
            beams[i].Initialize(damage, enemyTag, afterEffects, spawnpoints[i]);
            UpdateTransform();
        }
    }

    public override void Update()
    {
        base.Update();
        if (!target) return;
        if (!firing)
        {
            firing = true;
            StartCoroutine(Firing());
        }
    }

    void UpdateTransform()
    {
        for (int i = 0; i < spawnpoints.Length; i++)
        {
            beams[i].transform.SetParent(spawnpoints[i]);
            beams[i].transform.position = spawnpoints[i].position;
            beams[i].transform.rotation = spawnpoints[i].rotation;
        }
    }

    public override void ModuleActivated(ModuleAbility module)
    {
        print("module activated");
        base.ModuleActivated(module);
        for (int i = 0; i < GetActiveModules().Count; i++)
        {
            for (int j = 0; j < beams.Count; j++)
                beams[j].SetModule(GetActiveModules()[i]);
        }

        RemoveModules();
    }

    protected override void TargetUpdated()
    {
        base.TargetUpdated();
        for (int i = 0; i < beams.Count; i++)
        {
            beams[i].SetTarget(target, true, false);
        }
    }

    IEnumerator Firing()
    {
        for (int i = 0; i < beams.Count; i++)
        {
            beams[i].gameObject.SetActive(true);
        }

        yield return new WaitForSeconds(effectLength);

        for (int i = 0; i < beams.Count; i++)
        {
            beams[i].gameObject.SetActive(false);
        }

        yield return new WaitForSeconds(effectCooldown);

        firing = false;
    }
}